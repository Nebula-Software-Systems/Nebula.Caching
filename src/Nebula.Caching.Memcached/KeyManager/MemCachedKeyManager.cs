using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Memcached.Attributes;
using System.Reflection;

namespace Nebula.Caching.Memcached.KeyManager
{
    public class MemCachedKeyManager : IKeyManager
    {
        public async Task<string> GenerateCacheKeyAsync(MethodInfo executedMethodInfo, MethodInfo serviceMethodInfo, string[] parameters)
        {
            ArgumentNullException.ThrowIfNull(argument: parameters);
            return await HasCustomCacheNameDefinedAsync(serviceMethodInfo) ?
                                                            await GetCustomCacheNameAsync(serviceMethodInfo)
                                                            :
                                                            await GetDefaultCacheNameAsync(executedMethodInfo, parameters);
        }

        public Task<string> ConvertCacheKeyToConfigKeyAsync(string key)
        {
            return Task.Run(() =>
            {
                ArgumentNullException.ThrowIfNull(key);

                return (key.Replace(KeyConstants.MethodFullPathSeparator, KeyConstants.ConfigMethodFullPathSeparator))
                        .Replace(KeyConstants.MethodAndParametersSeparator, KeyConstants.ConfigMethodAndParametersSeparator);
            });
        }

        #region Utils
        private Task<bool> HasCustomCacheNameDefinedAsync(MethodInfo methodInfo)
        {
            return Task.Run(() =>
            {
                var executedMethodAttribute = methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(
                                            x => typeof(MemCachedCacheAttribute).IsAssignableFrom(x.GetType())
                                        );

                var castedExecutedMethodAttribute = executedMethodAttribute as MemCachedCacheAttribute;
                return castedExecutedMethodAttribute.CustomCacheName is not null;
            });
        }

        private Task<string> GetCustomCacheNameAsync(MethodInfo methodInfo)
        {
            return Task.Run(() =>
            {
                var executedMethodAttribute = methodInfo.GetCustomAttributes(true)
            .FirstOrDefault(
                                x => typeof(MemCachedCacheAttribute).IsAssignableFrom(x.GetType())
                            );

                var castedExecutedMethodAttribute = executedMethodAttribute as MemCachedCacheAttribute;

                return castedExecutedMethodAttribute.CustomCacheName;
            });
        }

        private Task<string> GetDefaultCacheNameAsync(MethodInfo methodInfo, string[] parameters)
        {
            return Task.Run(() =>
            {
                string methodParamsAggregated = string.Join(KeyConstants.MethodAndParametersSeparator, parameters);
                return $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}{(parameters.Length > 0 ? KeyConstants.MethodAndParametersSeparator : "")}{methodParamsAggregated}";
            });
        }
        #endregion
    }
}