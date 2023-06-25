using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.InMemory.Interceptors;

namespace Nebula.Caching.InMemory.Extensions.InterceptorExtensions
{
    public static class InterceptorExtensions
    {
        public static IServiceCollection AddInMemoryInterceptor(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryCacheInterceptor>();

            services.ConfigureDynamicProxy(config =>
            {
                config
                    .Interceptors
                    .AddServiced<InMemoryCacheInterceptor>();
            });
            return services;
        }
    }
}