using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Memcached.Interceptors;

namespace Nebula.Caching.MemCached.Extensions.InterceptorExtensions
{
    public static class InterceptorExtensions
    {
        public static IServiceCollection AddMemCachedInterceptor(this IServiceCollection services)
        {
            services.AddSingleton<MemCachedInterceptor>();

            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddServiced<MemCachedInterceptor>();
            });

            return services;
        }
    }
}