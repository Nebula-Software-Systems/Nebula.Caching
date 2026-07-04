using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Settings;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.InMemory.KeyManager;

namespace Nebula.Caching.InMemory.UtilsExtensions;

public static class CacheUtilsExtensions
{
    public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            BaseOptions? inMemoryOptions = serviceProvider.GetService<BaseOptions>();
            return new ContextUtils(new InMemoryKeyManager(), inMemoryOptions!);
        });

        return services;
    }
}
