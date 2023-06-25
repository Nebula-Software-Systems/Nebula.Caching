using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.InMemory.Settings;

namespace Nebula.Caching.InMemory.Extensions.InMemoryExtensions
{
    public static class InMemoryExtensions
    {
        public static IServiceCollection AddInMemoryExtensions(this IServiceCollection services, InMemoryConfigurations inMemoryConfigs)
        {
            CreateDefaultInMemoryConfigurationsIfNull(inMemoryConfigs);
            SetDefaultValuesBasedOnInMemoryConfigurations(inMemoryConfigs);
            InjectInMemoryOptionsObject(services, inMemoryConfigs);

            return services;
        }

        private static void CreateDefaultInMemoryConfigurationsIfNull(InMemoryConfigurations inMemoryConfigs)
        {
            if (inMemoryConfigs is null)
            {
                inMemoryConfigs = new InMemoryConfigurations
                {
                    ConfigurationSection = "InMemory"
                };
            }
        }

        private static void SetDefaultValuesBasedOnInMemoryConfigurations(InMemoryConfigurations inMemoryConfigs)
        {
            CacheDurationConstants.DefaultCacheDurationInSeconds = inMemoryConfigs.DefaultCacheDurationInSeconds;

            CacheConfigurationConstants.ConfigurationSection = inMemoryConfigs.ConfigurationSection;
        }

        private static void InjectInMemoryOptionsObject(IServiceCollection services, InMemoryConfigurations inMemoryConfigs)
        {
            services.AddSingleton<InMemoryOptions>(ctx =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var inMemoryOptions = configuration.GetSection(inMemoryConfigs.ConfigurationSection).Get<InMemoryOptions>();
                inMemoryOptions.ConfigurationRoot = inMemoryConfigs.ConfigurationSection;
                return inMemoryOptions;
            });
        }

    }
}