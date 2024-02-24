using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Memcached.Constants;
using Nebula.Caching.MemCached.Settings;

namespace Nebula.Caching.MemCached.Extensions.MemCachedExtensions
{
    public static class MemCachedExtensions
    {
        public static IServiceCollection AddMemCachedExtensions(this IServiceCollection services, MemCachedConfigurations? memcachedConfigs)
        {
            if (memcachedConfigs is null)
                memcachedConfigs = new();

            CacheConstants.DefaultCacheDurationInSeconds = memcachedConfigs.DefaultCacheDurationInSeconds;

            services.AddSingleton(ctx =>
            {
                var configuration = ctx.GetRequiredService<IConfiguration>();
                return configuration.GetSection(memcachedConfigs.ConfigurationSection).Get<MemCachedOptions>();
            });

            SetupMemCache(services);

            return services;
        }

        private static void SetupMemCache(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var memCachedOptions = serviceProvider.GetRequiredService<MemCachedOptions>();
            var memCacheServerSplit = memCachedOptions.CacheServiceUrl.Split(':');
            services.AddEnyimMemcached(
                o => o.Servers = new List<Server> 
                { 
                    new Server 
                    { 
                        Address = memCacheServerSplit[0],
                        Port = memCacheServerSplit[1] is null ? Constants.DefaultMemcachedPort : Int32.Parse(memCacheServerSplit[1]) 
                    } 
                }
            );
        }

    }
}