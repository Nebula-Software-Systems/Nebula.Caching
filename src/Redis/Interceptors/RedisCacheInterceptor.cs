using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Redis.Attributes;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Interceptors
{
    public class RedisCacheInterceptor : AbstractInterceptorAttribute
    {
        public RedisCacheInterceptor()
        {
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cache = 250;

            var stuff = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));

            if (stuff is RedisCacheAttribute attribute)
            {
                cache = attribute.CacheDuration;
            }

            var redis = context.ServiceProvider.GetService<IConnectionMultiplexer>();
            var db = redis.GetDatabase();
            await db.SetAddAsync("AttributeKey", $"New Key Value from Interceptor: {cache}");
            await db.KeyExpireAsync("AttributeKey", TimeSpan.FromSeconds(cache));
            await next(context);
        }
    }
}