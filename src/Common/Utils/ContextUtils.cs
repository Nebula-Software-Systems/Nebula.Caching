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
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true).FirstOrDefault(x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType()));
            var castedExecutedMethodAttribute = executedMethodAttribute as RedisCacheAttribute;
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
    }
}