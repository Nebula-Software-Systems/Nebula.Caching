using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Redis.Attributes;
using System.Reflection;

namespace Nebula.Caching.Redis.KeyManager
{
    public class RedisKeyManager : IKeyManager
    {

        public RedisKeyManager()
        {

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
            var executedMethodAttribute = methodInfo.GetCustomAttributes(true)
                                    .FirstOrDefault(
                                                        x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType())
                                                    );

            var castedExecutedMethodAttribute = executedMethodAttribute as RedisCacheAttribute;
            return castedExecutedMethodAttribute.CustomCacheName is not null;
        }

        public string GetCustomCacheName(MethodInfo methodInfo)
        {
            var executedMethodAttribute = methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(
                                            x => typeof(RedisCacheAttribute).IsAssignableFrom(x.GetType())
                                        );

            var castedExecutedMethodAttribute = executedMethodAttribute as RedisCacheAttribute;

            return castedExecutedMethodAttribute.CustomCacheName;
        }

        public string GetDefaultCacheName(MethodInfo methodInfo, string[] parameters)
        {
            string methodParamsAggregated = string.Join(KeyConstants.MethodAndParametersSeparator, parameters);
            return $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}{(parameters.Length > 0 ? KeyConstants.MethodAndParametersSeparator : "")}{methodParamsAggregated}";
        }

        public string ConvertCacheKeyToConfigKey(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return (key.Replace(KeyConstants.MethodFullPathSeparator, KeyConstants.ConfigMethodFullPathSeparator))
                    .Replace(KeyConstants.MethodAndParametersSeparator, KeyConstants.ConfigMethodAndParametersSeparator);
        }
    }
}