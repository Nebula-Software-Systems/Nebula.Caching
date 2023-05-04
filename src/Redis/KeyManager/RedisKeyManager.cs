using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;

namespace Nebula.Caching.Redis.KeyManager
{
    public class RedisKeyManager : IKeyManager
    {

        public RedisKeyManager()
        {

        }

        public string GenerateKey(MethodInfo methodInfo, string[] parameters)
        {
            ArgumentNullException.ThrowIfNull(argument: parameters);

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