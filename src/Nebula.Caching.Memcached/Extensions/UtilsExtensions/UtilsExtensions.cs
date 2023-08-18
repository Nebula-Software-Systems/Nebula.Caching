using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Memcached.KeyManager;
using Nebula.Caching.MemCached.Settings;
using System.Diagnostics.CodeAnalysis;

namespace Nebula.Caching.MemCached.Extensions.UtilsExtensions
{
    [ExcludeFromCodeCoverage]
    public static class UtilsExtensions
    {
        public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
        {
            services.AddScoped<IContextUtils>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var memCachedOptions = serviceProvider.GetService<MemCachedOptions>();
                return new ContextUtils(new MemCachedKeyManager(), configuration, memCachedOptions);
            });

            return services;
        }
    }
}