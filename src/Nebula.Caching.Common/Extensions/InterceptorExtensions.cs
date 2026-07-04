using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Interceptor;

namespace Nebula.Caching.Common.Extensions;

public static class InterceptorExtensions
{
    public static IServiceCollection AddNebulaCache(this IServiceCollection services)
    {
        services.AddSingleton<NebulaCacheInterceptor>();

        services.ConfigureDynamicProxy(config =>
        {
            config
                .Interceptors
                .AddServiced<NebulaCacheInterceptor>();
        });

        return services;
    }
}
