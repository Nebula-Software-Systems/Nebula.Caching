using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Extensions;
using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Redis.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddRedisChache(this IServiceCollection services, BaseOptions? configs = null)
    {
        return services
            .AddNebulaCache()
            .AddBaseOptions(configs)
            .AddManagerExtensions()
            .AddUtilsExtensions();
    }
}
