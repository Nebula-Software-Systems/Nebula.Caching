using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.CacheManager;
using Nebula.Caching.Redis.Interceptors;
using Nebula.Caching.Redis.KeyManager;
using Redis.Settings;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisChache(this IServiceCollection services, Configurations configs)
        {
            services.AddSingleton<RedisOptions>(ctx =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var redisOptions = configuration.GetSection(configs.ConfigurationSection).Get<RedisOptions>();
                redisOptions.ConfigurationRoot = configs.ConfigurationSection;
                return redisOptions;
            });

            if (configs.UseVanillaConfiguration)
            {
                services.AddSingleton<IConnectionMultiplexer>(ctx =>
                {
                    var options = ctx.GetService<RedisOptions>();
                    return ConnectionMultiplexer.Connect(options.CacheServiceUrl);
                });
            }

            services.AddSingleton<ICacheManager>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IDatabase>();
                return new RedisCacheManager(database);
            });

            services.AddSingleton<IKeyManager>(serviceProvider =>
            {
                return new RedisKeyManager();
            });

            services.AddSingleton<IContextUtils>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                return new ContextUtils(new RedisKeyManager(), configuration);
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

            return services;
        }
    }
}