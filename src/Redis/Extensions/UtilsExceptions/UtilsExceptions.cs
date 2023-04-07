using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.KeyManager;
using Redis.Settings;

namespace Redis.Extensions.UtilsExceptions
{
    public static class UtilsExceptions
    {
        public static IServiceCollection AddUtilsExceptions(this IServiceCollection services)
        {
            services.AddScoped<IContextUtils>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var redisOptions = serviceProvider.GetService<RedisOptions>();
                return new ContextUtils(new RedisKeyManager(), configuration, redisOptions);
            });

            return services;
        }
    }
}