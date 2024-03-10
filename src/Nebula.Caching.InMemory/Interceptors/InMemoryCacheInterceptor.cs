using AspectCore.DynamicProxy;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.InMemory.Attributes;
using Newtonsoft.Json;

namespace Nebula.Caching.InMemory.Interceptors
{
    public class InMemoryCacheInterceptor : AbstractInterceptorAttribute
    {
        private ICacheManager _cacheManager { get; set; }
        private IKeyManager _keyManager { get; set; }
        private IContextUtils _utils { get; set; }
        private AspectContext _context { get; set; }
        private AspectDelegate _next { get; set; }

        public InMemoryCacheInterceptor(IContextUtils utils, ICacheManager cacheManager, IKeyManager keyManager)
        {
            _cacheManager = cacheManager;
            _utils = utils;
            _keyManager = keyManager;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            _context = context;
            _next = next;
            if (await ExecutedMethodHasInMemoryCacheAttributeAsync()) await ExecuteMethodThatHasInMemoryCacheAttributeAsync().ConfigureAwait(false);
            else await ContinueExecutionForNonCacheableMethodAsync().ConfigureAwait(false);
        }

        private async Task<bool> ExecutedMethodHasInMemoryCacheAttributeAsync()
        {
            return await _utils.IsAttributeOfTypeAsync<InMemoryCacheAttribute>(_context);
        }

        private async Task ExecuteMethodThatHasInMemoryCacheAttributeAsync()
        {
            if (await CacheExistsAsync().ConfigureAwait(false))
            {
                await ReturnCachedValueAsync().ConfigureAwait(false);
            }
            else
            {
                await _next(_context).ConfigureAwait(false);
                await CacheValueAsync().ConfigureAwait(false);
            }
        }

        private async Task ContinueExecutionForNonCacheableMethodAsync()
        {
            await _next(_context).ConfigureAwait(false);
        }

        private async Task ReturnCachedValueAsync()
        {
            var value = await _cacheManager.GetAsync(await GenerateKeyAsync()).ConfigureAwait(false);

            if (_context.IsAsync())
            {
                dynamic objectType = _context.ServiceMethod.ReturnType.GetGenericArguments().First();
                dynamic deserializedObject = JsonConvert.DeserializeObject(value, objectType);
                _context.ReturnValue = Task.FromResult(deserializedObject);
            }
            else
            {
                var objectType = _context.ServiceMethod.ReturnType;
                _context.ReturnValue = JsonConvert.DeserializeObject(value, objectType);
            }
        }

        private async Task CacheValueAsync()
        {
            string value;

            if (_context.IsAsync())
            {
                Type returnType = _context.ReturnValue.GetType();
                value = JsonConvert.SerializeObject(await _context.UnwrapAsyncReturnValue().ConfigureAwait(false));
            }
            else
            {
                var returnValue = _context.ReturnValue;
                value = JsonConvert.SerializeObject(returnValue);
            }

            var key = await GenerateKeyAsync();
            var expiration = TimeSpan.FromSeconds(await _utils.GetCacheDurationAsync<InMemoryCacheAttribute>(key, _context));

            await _cacheManager.SetAsync(key, value, expiration).ConfigureAwait(false);
        }

        private async Task<bool> CacheExistsAsync()
        {
            return await _cacheManager.CacheExistsAsync(await GenerateKeyAsync()).ConfigureAwait(false);
        }

        private async Task<string> GenerateKeyAsync()
        {
            return await _keyManager.GenerateCacheKeyAsync(await _utils.GetExecutedMethodInfoAsync(_context), await _utils.GetServiceMethodInfoAsync(_context), await _utils.GetMethodParametersAsync(_context));
        }
    }
}