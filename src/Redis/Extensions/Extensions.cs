using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Redis.Interceptors;

namespace Nebula.Caching.Redis.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisChache(this IServiceCollection services)
        {
            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddTyped<RedisCacheInterceptor>();
            });
            return services;
        }
    }
}