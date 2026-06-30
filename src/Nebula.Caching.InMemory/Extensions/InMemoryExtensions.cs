using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.InMemory.Settings;
using Nebula.Caching.InMemory.UtilsExtensions;

namespace Nebula.Caching.InMemory.Extensions;

public static class InMemoryExtensions
{
    public static IServiceCollection AddInMemoryChache(this IServiceCollection services, InMemoryConfigurations? configs = null)
    {
        return services
            .AddInMemoryInterceptor()
            .AddInMemoryExtensions(configs)
            .AddManagerExtensions()
            .AddUtilsExtensions();
    }

    public static IServiceCollection AddInMemoryExtensions(this IServiceCollection services, InMemoryConfigurations? inMemoryConfigs)
    {
        if (inMemoryConfigs is null)
        {
            inMemoryConfigs = new InMemoryConfigurations
            {
                ConfigurationSection = "InMemory"
            };
        }

        CacheDurationConstants.DefaultCacheDurationInSeconds = inMemoryConfigs.DefaultCacheDurationInSeconds;

        CacheConfigurationConstants.ConfigurationSection = inMemoryConfigs.ConfigurationSection;

        services.AddSingleton<InMemoryOptions>(ctx =>
        {
            var configuration = ctx.GetService<IConfiguration>();
            var inMemoryOptions = configuration.GetSection(inMemoryConfigs.ConfigurationSection).Get<InMemoryOptions>();
            inMemoryOptions.ConfigurationRoot = inMemoryConfigs.ConfigurationSection;
            return inMemoryOptions;
        });

        return services;
    }
}
