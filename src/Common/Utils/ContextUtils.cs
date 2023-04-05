using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.Attributes;
using Redis.Settings;

namespace Nebula.Caching.Common.Utils
{
    public class ContextUtils : IContextUtils
    {

        private IKeyManager _keyManager;
        private IConfiguration _configuration;

        public ContextUtils(IKeyManager keyManager, IConfiguration configuration)
        {
            _keyManager = keyManager;
            _configuration = configuration;
        }

        public int GetCacheDuration<T>(string key, AspectContext context) where T : BaseAttribute
        {

            var cacheDict = _configuration.GetSection("Redis").Get<CacheKeyValuePairs>();
            var finalChangedKey = (key.Replace('.', '-')).Replace(":", "--");
            var cacheExpiration = cacheDict.CacheSettings.GetValueOrDefault(finalChangedKey);

            if (cacheExpiration != null || cacheExpiration > TimeSpan.Zero)
            {
                //value of cache expiration exists in the config files
                return (int)cacheExpiration.TotalSeconds;
            }


            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                            .FirstOrDefault(
                                                                                x => typeof(T).IsAssignableFrom(x.GetType())
                                                                            );
            var castedExecutedMethodAttribute = executedMethodAttribute as T;
            return castedExecutedMethodAttribute.CacheDuration;
        }

        public MethodInfo GetExecutedMethodInfo(AspectContext context)
        {
            return context.ImplementationMethod;
        }

        public string[] GetMethodParameters(AspectContext context)
        {
            var methodParams = context.Parameters.Select((object obj) =>
            {
                return obj.ToString();
            }).ToArray();
            return methodParams;
        }

        public bool IsAttributeOfType<T>(AspectContext context) where T : BaseAttribute
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                            .FirstOrDefault(
                                                            x => typeof(T).IsAssignableFrom(x.GetType())
                                                            );
            return executedMethodAttribute is T;
        }

    }
}