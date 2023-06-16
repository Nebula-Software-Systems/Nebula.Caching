using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.InMemory.Settings;

namespace Nebula.Caching.InMemory.Extensions.InMemoryExtensions
{
    public static class InMemoryExtensions
    {
        public static IServiceCollection AddInMemoryExtensions(this IServiceCollection services, InMemoryConfigurations? configs)
        {
            if (configs is null) configs = new InMemoryConfigurations();

            CacheDurationConstants.DefaultCacheDurationInSeconds = configs.DefaultCacheDurationInSeconds;

            services.AddSingleton<InMemoryOptions>(ctx =>
            {
                var configuration = ctx.GetService<IConfiguration>();
                var inMemoryOptions = configuration.GetSection(configs.ConfigurationSection).Get<InMemoryOptions>();
                inMemoryOptions.ConfigurationRoot = configs.ConfigurationSection;
                return inMemoryOptions;
            });

            return services;
        }
    }
}