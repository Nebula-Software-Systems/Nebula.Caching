using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.Attributes;
using Nebula.Caching.src.Common.Utils;
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
            if (ExecutedMethodHasRedisCacheAttribute(context)) await ExecuteMethodThatHasRedisCacheAttribute(context, next);
            else await ContinueExecutionForNonCacheableMethod(context, next);
        }

        private bool ExecutedMethodHasRedisCacheAttribute(AspectContext context)
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));
            return executedMethodAttribute is RedisCacheAttribute;
        }

        private async Task ExecuteMethodThatHasRedisCacheAttribute(AspectContext context, AspectDelegate next)
        {
            if (await CacheExistsAsync(context))
            {
                await ReturnCachedValueAsync(context);
            }
            else
            {
                await next(context);
                await CacheValueAsync(context);
            }
        }

        private async Task ContinueExecutionForNonCacheableMethod(AspectContext context, AspectDelegate next)
        {
            await next(context);
        }

        private async Task ReturnCachedValueAsync(AspectContext context)
        {
            context.ReturnValue = await _cacheManager.GetAsync(GenerateKey(context));
        }

        private async Task CacheValueAsync(AspectContext context)
        {
            await _cacheManager.SetAsync(GenerateKey(context), Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(_utils.GetCacheDuration(context)));
        }

        private async Task<bool> CacheExistsAsync(AspectContext context)
        {
            return await _cacheManager.CacheExistsAsync(GenerateKey(context));
        }

        private string GenerateKey(AspectContext context)
        {
            return _keyManager.GenerateKey(_utils.GetExecutedMethodInfo(context), _utils.GetMethodParameters(context));
        }
    }
}