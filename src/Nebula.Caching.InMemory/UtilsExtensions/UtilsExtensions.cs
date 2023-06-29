using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Compression;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.InMemory.KeyManager;
using Nebula.Caching.InMemory.Settings;

namespace Nebula.Caching.InMemory.UtilsExtensions
{
    public static class UtilsExtensions
    {
        public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
        {
            services.AddScoped<IContextUtils>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var inMemoryOptions = serviceProvider.GetService<InMemoryOptions>();
                return new ContextUtils(new InMemoryKeyManager(), configuration, inMemoryOptions);
            });

            services.AddScoped<GZipCompression>();

            return services;
        }
    }
}