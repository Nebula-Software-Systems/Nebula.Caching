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
                await next(context).ConfigureAwait(false);

                //cache executed method
                await CacheValue(context).ConfigureAwait(false);
            }
        }

        private void ReturnCachedValue(AspectContext context)
        {
            var redis = GetDatabase(context);
            string key = $"{context.ImplementationMethod.Name}";
            context.ReturnValue = redis.StringGet(key).ToString();
        }

        private async Task CacheValue(AspectContext context)
        {
            var cache = 250;

            var stuff = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));

            if (stuff is RedisCacheAttribute attribute)
            {
                cache = attribute.CacheDuration;
            }

            //var database = GetDatabase(context);
            string key = $"{context.ImplementationMethod.Name}";
            //database.StringSet(key, Convert.ToString(context.ReturnValue));
            //database.KeyExpire(key, TimeSpan.FromSeconds(cache));
            var cacheManager = context.ServiceProvider.GetService<ICacheManager>();
            await cacheManager.SetAsync(key, Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(cache));
        }

        private bool CacheExists(AspectContext context)
        {
            var database = GetDatabase(context);
            string key = $"{context.ImplementationMethod.Name}";
            return database.StringGet(key) != RedisValue.Null;
        }

        private IDatabase GetDatabase(AspectContext context)
        {
            return context.ServiceProvider.GetService<IDatabase>();
        }

    }
}