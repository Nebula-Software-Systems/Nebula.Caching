using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.Attributes;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Interceptors
{
    public class RedisCacheInterceptor : AbstractInterceptorAttribute
    {
        private ICacheManager _cacheManager { get; set; }
        private IKeyManager _keyManager { get; set; }
        private IContextUtils _utils { get; set; }

        public RedisCacheInterceptor(ICacheManager cacheManager, IKeyManager keyManager, IContextUtils utils)
        {
            _cacheManager = cacheManager;
            _keyManager = keyManager;
            _utils = utils;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (ExecutedMethodHasRedisCacheAttribute(context)) await ExecuteMethodThatHasRedisCacheAttribute(context, next).ConfigureAwait(false);
            else await ContinueExecutionForNonCacheableMethod(context, next).ConfigureAwait(false);
        }

        private bool ExecutedMethodHasRedisCacheAttribute(AspectContext context)
        {
            return _utils.IsAttributeOfType<RedisCacheAttribute>(context);
        }

        private async Task ExecuteMethodThatHasRedisCacheAttribute(AspectContext context, AspectDelegate next)
        {
            if (await CacheExistsAsync(context).ConfigureAwait(false))
            {
                await ReturnCachedValueAsync(context).ConfigureAwait(false);
            }
            else
            {
                await next(context).ConfigureAwait(false);
                await CacheValueAsync(context).ConfigureAwait(false);
            }
        }

        private async Task ContinueExecutionForNonCacheableMethod(AspectContext context, AspectDelegate next)
        {
            await next(context).ConfigureAwait(false);
        }

        private async Task ReturnCachedValueAsync(AspectContext context)
        {
            var value = await _cacheManager.GetAsync(GenerateKey(context)).ConfigureAwait(false);

            if (context.IsAsync())
            {
                dynamic objectType = context.ServiceMethod.ReturnType.GetGenericArguments().First();
                dynamic deserializedObject = JsonConvert.DeserializeObject(value, objectType);
                context.ReturnValue = Task.FromResult(deserializedObject);
            }
            else
            {
                var objectType = context.ServiceMethod.ReturnType;
                context.ReturnValue = JsonConvert.DeserializeObject(value, objectType);
            }
        }

        private async Task CacheValueAsync(AspectContext context)
        {
            string value = "";

            if (context.IsAsync())
            {
                Type returnType = context.ReturnValue.GetType();
                value = JsonConvert.SerializeObject(await context.UnwrapAsyncReturnValue().ConfigureAwait(false));
            }
            else
            {
                var returnValue = context.ReturnValue;
                value = JsonConvert.SerializeObject(returnValue);
            }

            await _cacheManager.SetAsync(GenerateKey(context), value, TimeSpan.FromSeconds(_utils.GetCacheDuration<RedisCacheAttribute>(GenerateKey(context), context))).ConfigureAwait(false);
        }

        private async Task<bool> CacheExistsAsync(AspectContext context)
        {
            return await _cacheManager.CacheExistsAsync(GenerateKey(context)).ConfigureAwait(false);
        }

        private string GenerateKey(AspectContext context)
        {
            return _keyManager.GenerateKey(_utils.GetExecutedMethodInfo(context), _utils.GetMethodParameters(context));
        }
    }
}