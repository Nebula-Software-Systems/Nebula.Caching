using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Redis.Attributes;

using StackExchange.Redis;

namespace Nebula.Caching.Redis.Interceptors
{
    public class RedisCacheInterceptor : AbstractInterceptorAttribute
    {
        private ICacheManager cacheManager { get; set; }

        public RedisCacheInterceptor()
        {
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            InitDependencies(context);
            if (CacheExists(context))
            {
                ReturnCachedValue(context);
            }
            else
            {
                await next(context);
                CacheValue(context);
            }
        }

        private void InitDependencies(AspectContext context)
        {
            cacheManager = context.ServiceProvider.GetService<ICacheManager>();
        }

        private void ReturnCachedValue(AspectContext context)
        {
            context.ReturnValue = cacheManager.Get(GenerateKey(context));
        }

        private void CacheValue(AspectContext context)
        {
            var cache = 250;

            var stuff = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));

            if (stuff is RedisCacheAttribute attribute)
            {
                cache = attribute.CacheDuration;
            }

            cacheManager.Set(GenerateKey(context), Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(cache));
        }

        private bool CacheExists(AspectContext context)
        {
            return cacheManager.CacheExists(GenerateKey(context));
        }

        private string GenerateKey(AspectContext context)
        {
            var methodParams = context.Parameters.Select((object obj) =>
            {
                return obj.ToString();
            }).ToArray();

            string methodParamsAggregated = string.Join(":", methodParams);

            return $"{context.ImplementationMethod.DeclaringType.FullName}:{context.ImplementationMethod.Name}:{methodParamsAggregated}";
        }
    }
}