using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.MemCached.Settings;

namespace Nebula.Caching.MemCached.Extensions.RedisExtensions
{
    public static class MemCachedExtensions
    {
        public static IServiceCollection AddMemCachedExtensions(this IServiceCollection services, MemCachedConfigurations configs)
        {
            CacheDurationConstants.DefaultCacheDurationInSeconds = configs.DefaultCacheDurationInSeconds;

            CacheConfigurationConstants.ConfigurationSection = configs.ConfigurationSection;

            services.AddSingleton<MemCachedOptions>(ctx =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var memCachedOptions = configuration.GetSection(configs.ConfigurationSection).Get<MemCachedOptions>();
                memCachedOptions.ConfigurationRoot = configs.ConfigurationSection;
                return memCachedOptions;
            });

            services.AddLogging();

            services.AddEnyimMemcached(o => o.Servers = new List<Server> { new Server { Address = "localhost", Port = 11211 } });

            return services;
        }

    }
}