using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Settings;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Memcached.KeyManager;

namespace Nebula.Caching.Memcached.Extensions;

public static class UtilsExtensions
{
    public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            BaseOptions? memCachedOptions = serviceProvider.GetService<BaseOptions>();
            return new ContextUtils(new MemCachedKeyManager(), memCachedOptions!);
        });

        return services;
    }
}
