using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Redis.Settings;
using Redis.Settings;
using StackExchange.Redis;

namespace Redis.Extensions.RedisExtensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisExtensions(this IServiceCollection services, RedisConfigurations configs)
        {
            CacheDurationConstants.DefaultCacheDurationInSeconds = configs.DefaultCacheDurationInSeconds;

            CacheConfigurationConstants.ConfigurationSection = "RedisConfig";

            services.AddSingleton<RedisOptions>(ctx =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var redisOptions = configuration.GetSection(configs.ConfigurationSection).Get<RedisOptions>();
                redisOptions.ConfigurationRoot = configs.ConfigurationSection;
                return redisOptions;
            });

            if (configs.ConfigurationFlavour == RedisConfigurationFlavour.Vanilla)
            {
                services.AddSingleton<IConnectionMultiplexer>(ctx =>
                {
                    var options = ctx.GetService<RedisOptions>();
                    return ConnectionMultiplexer.Connect(options.CacheServiceUrl, configs.Log);
                });
            }
            else if (configs.ConfigurationFlavour == RedisConfigurationFlavour.Configured)
            {
                if (configs.Configuration != null)
                {
                    services.AddSingleton<IConnectionMultiplexer>(ctx =>
                    {
                        return ConnectionMultiplexer.Connect(configs.Configuration, configs.Log);
                    });
                }
                else
                {
                    services.AddSingleton<IConnectionMultiplexer>(ctx =>
                    {
                        var options = ctx.GetService<RedisOptions>();
                        return ConnectionMultiplexer.Connect(options.CacheServiceUrl, configs.Configure, configs.Log);
                    });
                }
            }

            services.AddSingleton<IDatabase>(serviceProvider =>
            {
                var redis = serviceProvider.GetService<IConnectionMultiplexer>();
                return redis.GetDatabase();
            });

            return services;
        }

    }
}