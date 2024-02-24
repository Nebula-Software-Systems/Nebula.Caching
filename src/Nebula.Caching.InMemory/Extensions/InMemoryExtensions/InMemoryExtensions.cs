using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.InMemory.Settings;

namespace Nebula.Caching.InMemory.Extensions.InMemoryExtensions
{
    public static class InMemoryExtensions
    {
        public static IServiceCollection AddInMemoryExtensions(this IServiceCollection services, InMemoryConfigurations? inMemoryConfigs)
        {
            if (inMemoryConfigs is null)
                inMemoryConfigs = new();

            CacheConstants.DefaultCacheDurationInSeconds = inMemoryConfigs.DefaultCacheDurationInSeconds;

            services.AddSingleton<InMemoryOptions>(ctx =>
            {
                var configuration = ctx.GetRequiredService<IConfiguration>();
                var inMemoryOptions = configuration.GetSection(inMemoryConfigs.ConfigurationSection).Get<InMemoryOptions>();
                return inMemoryOptions;
            });

            return services;
        }
    }
}