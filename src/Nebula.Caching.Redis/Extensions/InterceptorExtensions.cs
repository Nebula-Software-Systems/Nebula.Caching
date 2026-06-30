using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Redis.Interceptors;

namespace Nebula.Caching.Redis.Extensions;

public static class InterceptorExtensions
{
    public static IServiceCollection AddRedisInterceptor(this IServiceCollection services)
    {
        services.AddSingleton<RedisCacheInterceptorAttribute>();

        services.ConfigureDynamicProxy(config =>
        {
            config
                .Interceptors
                .AddServiced<RedisCacheInterceptorAttribute>();
        });
        return services;
    }
}