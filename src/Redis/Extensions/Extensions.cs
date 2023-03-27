using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Redis.CacheManager;
using Nebula.Caching.Redis.Interceptors;
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

            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddTyped<RedisCacheInterceptor>();
            });

            services.AddSingleton<IDatabase>(serviceProvider =>
            {
                var redis = serviceProvider.GetService<IConnectionMultiplexer>();
                return redis.GetDatabase();
            });

            return services;
        }
    }
}