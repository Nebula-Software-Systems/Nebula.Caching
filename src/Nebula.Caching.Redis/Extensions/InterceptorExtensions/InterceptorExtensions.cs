using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Redis.Interceptors;

namespace Redis.Extensions.InterceptorExtensions
{
    public static class InterceptorExtensions
    {
        public static IServiceCollection AddRedisInterceptor(this IServiceCollection services)
        {
            services.AddSingleton<RedisCacheInterceptor>();

            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddServiced<RedisCacheInterceptor>();
            });
            return services;
        }
    }
}