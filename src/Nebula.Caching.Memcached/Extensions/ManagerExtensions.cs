using Enyim.Caching;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Memcached.CacheManager;

namespace Nebula.Caching.Memcached.Extensions;

public static class ManagerExtensions
{
    public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
    {
        IMemcachedClient? memCachedServer = services.BuildServiceProvider().GetService<IMemcachedClient>();

        services.AddScoped<ICacheManager>(_ => new MemCachedCacheManager(memCachedServer));

        return services;
    }

}
