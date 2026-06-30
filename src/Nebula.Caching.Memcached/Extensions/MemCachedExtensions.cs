using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Memcached.Settings;

namespace Nebula.Caching.Memcached.Extensions;

public static class MemCachedExtensions
{
    public static IServiceCollection AddMemCachedChache(this IServiceCollection services, MemCachedConfigurations configs)
    {
        return services
            .AddMemCachedInterceptor()
            .AddMemCachedExtensions(configs)
            .AddManagerExtensions()
            .AddUtilsExtensions();
    }

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

        SetupMemCache(services);

        return services;
    }

    private static void SetupMemCache(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var memCachedOptions = serviceProvider.GetRequiredService<MemCachedOptions>();
        string[] memCacheServerSplit = memCachedOptions.CacheServiceUrl.Split(':');
        services.AddEnyimMemcached(o => o.Servers = new List<Server> { new Server { Address = memCacheServerSplit[0], Port = Int32.Parse(memCacheServerSplit[1]) } });
    }

}
