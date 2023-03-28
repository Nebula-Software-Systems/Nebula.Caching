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
            if (CacheExists(context))
            {
                ReturnCachedValue(context);
            }
            else
            {
                await next(context);
                CacheValue(context);
            }
        }

        private void ReturnCachedValue(AspectContext context)
        {
            context.ReturnValue = _cacheManager.Get(GenerateKey(context));
        }

        private void CacheValue(AspectContext context)
        {
            _cacheManager.Set(GenerateKey(context), Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(_utils.GetCacheDuration(context)));
        }

        private bool CacheExists(AspectContext context)
        {
            return _cacheManager.CacheExists(GenerateKey(context));
        }

        private string GenerateKey(AspectContext context)
        {
            return _keyManager.GenerateKey(_utils.GetExecutedMethodInfo(context), _utils.GetMethodParameters(context));
        }
    }
}