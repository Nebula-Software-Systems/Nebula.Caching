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
        public static IServiceCollection AddRedisExtensions(this IServiceCollection services, RedisConfigurations? redisConfigs)
        {
            if (redisConfigs is null)
                redisConfigs = new();

            CacheConstants.DefaultCacheDurationInSeconds = redisConfigs.DefaultCacheDurationInSeconds;

            services.AddSingleton(ctx =>
            {
                var configuration = ctx.GetRequiredService<IConfiguration>(); 
                return configuration.GetSection(redisConfigs.ConfigurationSection).Get<RedisOptions>();
            });

            if (redisConfigs.ConfigurationFlavour == RedisConfigurationFlavour.Vanilla)
            {
                services.AddSingleton<IConnectionMultiplexer>(ctx =>
                {
                    var options = ctx.GetRequiredService<RedisOptions>();
                    return ConnectionMultiplexer.Connect(options.CacheServiceUrl, redisConfigs.Log);
                });
            }
            else if (redisConfigs.ConfigurationFlavour == RedisConfigurationFlavour.Configured)
            {
                if (redisConfigs.Configuration != null)
                {
                    services.AddSingleton<IConnectionMultiplexer>(ctx =>
                    {
                        return ConnectionMultiplexer.Connect(redisConfigs.Configuration, redisConfigs.Log);
                    });
                }
                else
                {
                    services.AddSingleton<IConnectionMultiplexer>(ctx =>
                    {
                        var options = ctx.GetRequiredService<RedisOptions>();
                        return ConnectionMultiplexer.Connect(options.CacheServiceUrl, redisConfigs.Configure, redisConfigs.Log);
                    });
                }
            }

            services.AddSingleton(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
            });

            return services;
        }

    }
}