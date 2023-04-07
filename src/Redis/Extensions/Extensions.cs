using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.CacheManager;
using Nebula.Caching.Redis.Interceptors;
using Nebula.Caching.Redis.KeyManager;
using Redis.Extensions.InterceptorExtensions;
using Redis.Extensions.ManagerExtensions;
using Redis.Extensions.RedisExtensions;
using Redis.Extensions.UtilsExceptions;
using Redis.Settings;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRedisChache(this IServiceCollection services, Configurations configs)
        {
            return services
                        .AddRedisInterceptor()
                        .AddRedisExtensions(configs)
                        .AddManagerExtensions()
                        .AddUtilsExceptions();
        }
    }
}