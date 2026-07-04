using AspectCore.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Common.Extensions;

public static class CommonExtensions
{
    public static IHostBuilder UseNebulaCache(this IHostBuilder builderHost)
    {
        return builderHost.UseDynamicProxy();
    }

    public static IServiceCollection AddBaseOptions(this IServiceCollection services, BaseOptions? configs = null)
    {
        services.AddSingleton<BaseOptions>(serviceProvider =>
        {
            if(configs is not null) return configs;

            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            configs = new BaseOptions();
            BaseOptions? baseOptions = configuration.GetSection(configs!.ConfigurationRoot).Get<BaseOptions>();
            return baseOptions ?? configs;
        });

        return services;
    }
}
