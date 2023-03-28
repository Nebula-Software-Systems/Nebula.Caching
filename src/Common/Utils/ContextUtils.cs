using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.Attributes;

namespace Nebula.Caching.src.Common.Utils
{
    public class ContextUtils : IContextUtils
    {

        private IKeyManager _keyManager;

        public ContextUtils(IKeyManager keyManager)
        {
            _keyManager = keyManager;
        }

        public int GetCacheDuration(AspectContext context)
        {
            var cacheDuration = 30;

            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));

            if (executedMethodAttribute is RedisCacheAttribute attribute)
            {
                cacheDuration = attribute.CacheDuration;
            }

            return cacheDuration;
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
    }
}