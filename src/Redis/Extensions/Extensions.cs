using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.CacheManager;
using Nebula.Caching.Redis.Interceptors;
using Nebula.Caching.Redis.KeyManager;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisChache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheManager>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IDatabase>();
                return new RedisCacheManager(database);
            });

            services.AddSingleton<RedisCacheInterceptor>();

            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddServiced<RedisCacheInterceptor>();
            });

            services.AddSingleton<IDatabase>(serviceProvider =>
            {
                var redis = serviceProvider.GetService<IConnectionMultiplexer>();
                return redis.GetDatabase();
            });

            //services.AddSingleton<IKeyManager, RedisKeyManager>();

            return services;
        }
    }
}