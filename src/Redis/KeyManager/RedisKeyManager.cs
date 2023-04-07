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
            string methodParamsAggregated = string.Join(KeyConstants.MethodAndParametersSeparator, parameters);
            return $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}{(parameters.Length > 0 ? KeyConstants.MethodAndParametersSeparator : "")}{methodParamsAggregated}";
        }

        public string ConvertCacheKeyToConfigKey(string key)
        {
            return (key.Replace(KeyConstants.MethodFullPathSeparator, KeyConstants.ConfigMethodFullPathSeparator))
                    .Replace(KeyConstants.MethodAndParametersSeparator, KeyConstants.ConfigMethodAndParametersSeparator);
        }

        public string CreateGenericCacheKey(AspectContext context)
        {
            List<string> genericParamsList = new List<string>();

            foreach (var param in context.GetParameters())
            {
                var genericParam = GenerateGeneriConfigCacheParameter(param.Name);
                genericParamsList.Add(genericParam);
            }

            var key = GenerateKey(context.ImplementationMethod, genericParamsList.ToArray());
            return ConvertCacheKeyToConfigKey(key);
        }

        public string GenerateGeneriConfigCacheParameter(string parameter)
        {
            return $"{{{parameter}}}";
        }

    }
}