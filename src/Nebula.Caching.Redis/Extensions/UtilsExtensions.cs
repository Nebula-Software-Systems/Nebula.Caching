using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.KeyManager;
using Nebula.Caching.Redis.Settings;

namespace Nebula.Caching.Redis.Extensions;

public static class UtilsExtensions
{
    public static IServiceCollection AddUtilsExtensions(this IServiceCollection services)
    {
        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            var redisOptions = serviceProvider.GetService<RedisOptions>();
            return new ContextUtils(new RedisKeyManager(), redisOptions);
        });

        return services;
    }
}