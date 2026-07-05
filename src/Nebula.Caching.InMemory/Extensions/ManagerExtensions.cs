using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.InMemory.CacheManager;

namespace Nebula.Caching.InMemory.Extensions;

public static class ManagerExtensions
{
    public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddScoped<ICacheManager>(serviceProvider =>
        {
            IMemoryCache memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            return new InMemoryCacheManager(memoryCache);
        });

        return services;
    }
}
