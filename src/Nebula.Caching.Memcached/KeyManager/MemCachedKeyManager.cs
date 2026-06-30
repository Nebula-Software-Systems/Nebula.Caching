using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Memcached.Attributes;
using System.Reflection;

namespace Nebula.Caching.Memcached.KeyManager;

public class MemCachedKeyManager : IKeyManager
{
    public string GenerateKey(MethodInfo executedMethodInfo, MethodInfo serviceMethodInfo, string[] parameters)
    {
        ArgumentNullException.ThrowIfNull(argument: parameters);
        return HasCustomCacheNameDefined(serviceMethodInfo) ?
            GetCustomCacheName(serviceMethodInfo)
            :
            GetDefaultCacheName(executedMethodInfo, parameters);
    }

    private static bool HasCustomCacheNameDefined(MethodInfo methodInfo)
    {
        object? executedMethodAttribute = methodInfo.GetCustomAttributes(true)
            .FirstOrDefault(
                x => (x is MemCachedCacheAttribute));

        var castedExecutedMethodAttribute = executedMethodAttribute as MemCachedCacheAttribute;
        return castedExecutedMethodAttribute.CustomCacheName is not null;
    }

    private static string GetCustomCacheName(MethodInfo methodInfo)
    {
        object? executedMethodAttribute = methodInfo.GetCustomAttributes(true)
            .FirstOrDefault(
                x => (x is MemCachedCacheAttribute));

        var castedExecutedMethodAttribute = executedMethodAttribute as MemCachedCacheAttribute;

        return castedExecutedMethodAttribute.CustomCacheName;
    }

    private static string GetDefaultCacheName(MethodInfo methodInfo, string[] parameters)
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
