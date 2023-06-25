using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.InMemory.Attributes;

namespace Nebula.Caching.InMemory.KeyManager
{
    public class InMemoryKeyManager : IKeyManager
    {
        public string ConvertCacheKeyToConfigKey(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return (key.Replace(KeyConstants.MethodFullPathSeparator, KeyConstants.ConfigMethodFullPathSeparator))
                    .Replace(KeyConstants.MethodAndParametersSeparator, KeyConstants.ConfigMethodAndParametersSeparator);
        }

        public string GenerateKey(MethodInfo executedMethodInfo, MethodInfo serviceMethodInfo, string[] parameters)
        {
            ArgumentNullException.ThrowIfNull(argument: parameters);
            return HasCustomCacheNameDefined(serviceMethodInfo) ?
                                                            GetCustomCacheName(serviceMethodInfo)
                                                            :
                                                            GetDefaultCacheName(executedMethodInfo, parameters);
        }

        public bool HasCustomCacheNameDefined(MethodInfo methodInfo)
        {
            return GetCustomCacheName(methodInfo) is not null;
        }

        public string GetCustomCacheName(MethodInfo methodInfo)
        {
            var executedMethodAttribute = methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(
                                            x => typeof(InMemoryCacheAttribute).IsAssignableFrom(x.GetType())
                                        );

            var castedExecutedMethodAttribute = executedMethodAttribute as InMemoryCacheAttribute;

            return castedExecutedMethodAttribute.CustomCacheName;
        }

        public string GetDefaultCacheName(MethodInfo methodInfo, string[] parameters)
        {
            string methodParamsAggregated = string.Join(KeyConstants.MethodAndParametersSeparator, parameters);
            return $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}{(parameters.Length > 0 ? KeyConstants.MethodAndParametersSeparator : "")}{methodParamsAggregated}";
        }
    }
}