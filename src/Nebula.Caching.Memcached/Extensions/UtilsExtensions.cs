using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Memcached.KeyManager;
using Nebula.Caching.Memcached.Settings;

namespace Nebula.Caching.Memcached.Extensions;

public static class UtilsExtensions
{
    public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            var memCachedOptions = serviceProvider.GetService<MemCachedOptions>();
            return new ContextUtils(new MemCachedKeyManager(), memCachedOptions);
        });

        return services;
    }
}
