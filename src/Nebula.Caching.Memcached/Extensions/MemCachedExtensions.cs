using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Extensions;
using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Memcached.Extensions;

public static class MemCachedExtensions
{
    public static IServiceCollection AddMemCachedChache(this IServiceCollection services, BaseOptions? configs = null)
    {
        return services
            .AddNebulaCache()
            .AddBaseOptions(configs)
            .AddManagerExtensions()
            .AddUtilsExtensions();
    }
}
