using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.Attributes;

namespace Nebula.Caching.Common.Utils
{
    public class ContextUtils : IContextUtils
    {

        private IKeyManager _keyManager;

        public ContextUtils(IKeyManager keyManager)
        {
            _keyManager = keyManager;
        }

        public int GetCacheDuration<T>(AspectContext context) where T : BaseAttribute
        {
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