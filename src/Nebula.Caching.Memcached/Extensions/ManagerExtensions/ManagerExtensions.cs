using Enyim.Caching;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Memcached.CacheManager;
using Nebula.Caching.Memcached.KeyManager;

namespace Nebula.Caching.MemCached.Extensions.ManagerExtensions
{
    public static class ManagerExtensions
    {
        public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
        {
            var memCachedServer = services.BuildServiceProvider().GetService<IMemcachedClient>();

            services.AddScoped<ICacheManager>(serviceProvider =>
            {
                return new MemCachedCacheManager(memCachedServer);
            });

            services.AddScoped<IKeyManager>(serviceProvider =>
            {
                return new MemCachedKeyManager();
            });

            return services;
        }

    }
}