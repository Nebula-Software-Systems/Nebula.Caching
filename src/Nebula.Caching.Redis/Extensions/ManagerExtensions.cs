using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.CacheManager;
using Nebula.Caching.Redis.KeyManager;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Extensions;

public static class ManagerExtensions
{

    public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
    {
        services.AddScoped<ICacheManager>(serviceProvider =>
        {
            IConnectionMultiplexer? connectionMultiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            IDatabase? database = connectionMultiplexer.GetDatabase();
            return new RedisCacheManager(database);
        });

        services.AddScoped<IKeyManager>(_ => new RedisKeyManager());

        return services;
    }

}
