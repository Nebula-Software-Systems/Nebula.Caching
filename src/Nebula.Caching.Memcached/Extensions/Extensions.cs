using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.MemCached.Extensions.InterceptorExtensions;
using Nebula.Caching.MemCached.Extensions.ManagerExtensions;
using Nebula.Caching.MemCached.Extensions.RedisExtensions;
using Nebula.Caching.MemCached.Extensions.UtilsExtensions;
using Nebula.Caching.MemCached.Settings;
using System.Diagnostics.CodeAnalysis;

namespace Nebula.Caching.MemCached.Extensions
{
    [ExcludeFromCodeCoverage]
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