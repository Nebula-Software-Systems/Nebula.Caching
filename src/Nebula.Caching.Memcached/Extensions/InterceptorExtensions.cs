using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Memcached.Interceptors;

namespace Nebula.Caching.Memcached.Extensions;

public static class InterceptorExtensions
{
    public static IServiceCollection AddMemCachedInterceptor(this IServiceCollection services)
    {
        services.AddSingleton<MemCachedInterceptorAttribute>();

        services.ConfigureDynamicProxy(config =>
        {
            config
                .Interceptors
                .AddServiced<MemCachedInterceptorAttribute>();
        });

        return services;
    }
}