using Enyim.Caching;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Memcached.CacheManager;
using Nebula.Caching.Memcached.KeyManager;
using StackExchange.Redis;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Nebula.Caching.MemCached.Extensions.ManagerExtensions
{
    [ExcludeFromCodeCoverage]
    public static class ManagerExtensions
    {

        public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
        {
            var memCachedServer2 = services.BuildServiceProvider().GetService<IMemcachedClient>();


            services.AddScoped<ICacheManager>(serviceProvider =>
            {
                //TODO
                var memCachedServer = memCachedServer2;
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