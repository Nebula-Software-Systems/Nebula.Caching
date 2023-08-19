using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.InMemory.Extensions.InMemoryExtensions;
using Nebula.Caching.InMemory.Extensions.InterceptorExtensions;
using Nebula.Caching.InMemory.Extensions.ManagerExtensions;
using Nebula.Caching.InMemory.Settings;
using Nebula.Caching.InMemory.UtilsExtensions;

namespace Nebula.Caching.InMemory.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddInMemoryChache(this IServiceCollection services, InMemoryConfigurations? configs = null)
        {
            return services
                        .AddInMemoryInterceptor()
                        .AddInMemoryExtensions(configs)
                        .AddManagerExtensions()
                        .AddUtilsExtensions();
        }
    }
}