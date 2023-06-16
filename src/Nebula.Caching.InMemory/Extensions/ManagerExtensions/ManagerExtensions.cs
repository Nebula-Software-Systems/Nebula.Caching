using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.InMemory.CacheManager;
using Nebula.Caching.InMemory.KeyManager;

namespace Nebula.Caching.InMemory.Extensions.ManagerExtensions
{
    public static class ManagerExtensions
    {
        public static IServiceCollection AddManagerExtensions(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped<ICacheManager>(serviceProvider =>
            {
                var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
                return new InMemoryCacheManager(memoryCache);
            });

            services.AddScoped<IKeyManager>(serviceProvider =>
            {
                return new InMemoryKeyManager();
            });

            return services;
        }
    }
}