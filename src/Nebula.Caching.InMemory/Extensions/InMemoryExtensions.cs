using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Extensions;
using Nebula.Caching.Common.Settings;
using Nebula.Caching.InMemory.UtilsExtensions;

namespace Nebula.Caching.InMemory.Extensions;

public static class InMemoryExtensions
{
    public static IServiceCollection AddInMemoryChache(this IServiceCollection services, BaseOptions? configs = null)
    {
        return services
            .AddNebulaCache()
            .AddBaseOptions(configs)
            .AddManagerExtensions()
            .AddUtilsExtensions();
    }
}
