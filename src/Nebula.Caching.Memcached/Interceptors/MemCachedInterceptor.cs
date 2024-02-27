using AspectCore.DynamicProxy;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Memcached.Attributes;
using Newtonsoft.Json;

namespace Nebula.Caching.Memcached.Interceptors
{
    public class MemCachedInterceptor : AbstractInterceptorAttribute
    {
        private ICacheManager _cacheManager { get; set; }
        private IKeyManager _keyManager { get; set; }
        private IContextUtils _utils { get; set; }
        private AspectContext context { get; set; }
        private AspectDelegate next { get; set; }

        public MemCachedInterceptor(IContextUtils utils, ICacheManager cacheManager, IKeyManager keyManager)
        {
            _cacheManager = cacheManager;
            _utils = utils;
            _keyManager = keyManager;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            this.context = context;
            this.next = next;
            if (await ExecutedMethodHasMemCachedAttributeAsync()) await ExecuteMethodThatHasMemCachedAttributeAsync().ConfigureAwait(false);
            else await ContinueExecutionForNonCacheableMethodAsync().ConfigureAwait(false);
        }

        private async Task<bool> ExecutedMethodHasMemCachedAttributeAsync()
        {
            return await _utils.IsAttributeOfTypeAsync<MemCachedCacheAttribute>(context);
        }

        private async Task ExecuteMethodThatHasMemCachedAttributeAsync()
        {
            if (await CacheExistsAsync().ConfigureAwait(false))
            {
                await ReturnCachedValueAsync().ConfigureAwait(false);
            }
            else
            {
                await next(context).ConfigureAwait(false);
                await CacheValueAsync().ConfigureAwait(false);
            }
        }

        private async Task ContinueExecutionForNonCacheableMethodAsync()
        {
            await next(context).ConfigureAwait(false);
        }

        private async Task ReturnCachedValueAsync()
        {
            var value = await _cacheManager.GetAsync(await GenerateKeyAsync()).ConfigureAwait(false);

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

        private async Task CacheValueAsync()
        {
            string value;

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

            var key = await GenerateKeyAsync();

            var expiration = TimeSpan.FromSeconds(await _utils.GetCacheDurationAsync<MemCachedCacheAttribute>(key, context));

            await _cacheManager.SetAsync(key, value, expiration).ConfigureAwait(false);
        }

        private async Task<bool> CacheExistsAsync()
        {
            return await _cacheManager.CacheExistsAsync(await GenerateKeyAsync()).ConfigureAwait(false);
        }

        private async Task<string> GenerateKeyAsync()
        {
            return await _keyManager.GenerateCacheKeyAsync(await _utils.GetExecutedMethodInfoAsync(context), await _utils.GetServiceMethodInfoAsync(context), await _utils.GetMethodParametersAsync(context));
        }
    }
}