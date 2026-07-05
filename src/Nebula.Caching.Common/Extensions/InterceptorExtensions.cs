using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Interceptor;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Settings;
using Nebula.Caching.Common.Utils;

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

        services.AddScoped<IContextUtils>(serviceProvider =>
        {
            IKeyManager keyManager = serviceProvider.GetRequiredService<IKeyManager>();
            BaseOptions baseOptions = serviceProvider.GetRequiredService<BaseOptions>();
            return new ContextUtils(keyManager, baseOptions);
        });

        return services;
    }
}
