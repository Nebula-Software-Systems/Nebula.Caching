using Enyim.Caching;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Memcached.CacheManager;
using Nebula.Caching.Memcached.KeyManager;

namespace Nebula.Caching.Memcached.Extensions;

public static class ManagerExtensions
{
    public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
    {
        IMemcachedClient? memCachedServer = services.BuildServiceProvider().GetService<IMemcachedClient>();

        services.AddScoped<ICacheManager>(_ =>
        {
            return new MemCachedCacheManager(memCachedServer);
        });

        services.AddScoped<IKeyManager>(_ =>
        {
            return new MemCachedKeyManager();
        });

        return services;
    }

}