using System.Runtime.CompilerServices;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Redis.Attributes;

using StackExchange.Redis;

namespace Nebula.Caching.Redis.Interceptors
{
    public class RedisCacheInterceptor : AbstractInterceptorAttribute
    {

        //public ICacheManager CacheManager { get; set; }

        public RedisCacheInterceptor(/*ICacheManager cacheManager*/)
        {
            //CacheManager = cacheManager;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {

            //await CacheManager.SetAsync("key", "value", TimeSpan.FromSeconds(59));

            if (CacheExists(context))
            {
                ReturnCachedValue(context);
            }
            else
            {
                //Cache does not exist

                //execute method
                await next(context);

                //cache executed method
                CacheValue(context);
            }
        }

        private void ReturnCachedValue(AspectContext context)
        {
            string key = $"{context.ImplementationMethod.Name}";
            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
            context.ReturnValue = cacheManager.Get(key);
        }

        private void CacheValue(AspectContext context)
        {
            var cache = 250;

            var stuff = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));

            if (stuff is RedisCacheAttribute attribute)
            {
                cache = attribute.CacheDuration;
            }

            string key = $"{context.ImplementationMethod.Name}";
            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
            cacheManager.Set(key, Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(cache));
        }

        private bool CacheExists(AspectContext context)
        {
            string key = $"{context.ImplementationMethod.Name}";
            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
            return cacheManager.CacheExists(key);
        }

    }
}