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

        //public ICacheManager CacheManager { get; set; }

        public RedisCacheInterceptor(/*ICacheManager cacheManager*/)
        {
            //CacheManager = cacheManager;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {

            //await CacheManager.SetAsync("key", "value", TimeSpan.FromSeconds(59));

            if (CacheExists(context))
            {
                ReturnCachedValue(context);
            }
            else
            {
                //Cache does not exist

                //execute method
                await next(context);

                //cache executed method
                CacheValue(context);
            }
        }

        private void ReturnCachedValue(AspectContext context)
        {
            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
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

            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
            cacheManager.Set(GenerateKey(context), Convert.ToString(context.ReturnValue), TimeSpan.FromSeconds(cache));
        }

        private bool CacheExists(AspectContext context)
        {
            ICacheManager? cacheManager = context.ServiceProvider.GetService<ICacheManager>();
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