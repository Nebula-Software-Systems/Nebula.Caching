using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Redis.Settings;
using Redis.Extensions.InterceptorExtensions;
using Redis.Extensions.ManagerExtensions;
using Redis.Extensions.RedisExtensions;
using Redis.Extensions.UtilsExtensions;

namespace Nebula.Caching.Redis.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisChache(this IServiceCollection services, RedisConfigurations configs)
        {
            return services
                        .AddRedisInterceptor()
                        .AddRedisExtensions(configs)
                        .AddManagerExtensions()
                        .AddUtilsExtensions();
        }
    }
}