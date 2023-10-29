using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.MemCached.Extensions.InterceptorExtensions;
using Nebula.Caching.MemCached.Extensions.ManagerExtensions;
using Nebula.Caching.MemCached.Extensions.MemCachedExtensions;
using Nebula.Caching.MemCached.Extensions.UtilsExtensions;
using Nebula.Caching.MemCached.Settings;

namespace Nebula.Caching.MemCached.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddMemCachedChache(this IServiceCollection services, MemCachedConfigurations configs)
        {
            return services
                        .AddMemCachedInterceptor()
                        .AddMemCachedExtensions(configs)
                        .AddManagerExtensions()
                        .AddUtilsExtensions();
        }
    }
}