using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Settings;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.KeyManager;

namespace Nebula.Caching.Redis.Extensions;

public static class UtilsExtensions
{
    public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            BaseOptions? redisOptions = serviceProvider.GetService<BaseOptions>();
            return new ContextUtils(new RedisKeyManager(), redisOptions!);
        });

        return services;
    }
}
