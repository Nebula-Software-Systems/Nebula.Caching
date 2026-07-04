using System.Reflection;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Common.KeyManager;

namespace Nebula.Caching.Redis.KeyManager;

public class RedisKeyManager : IKeyManager
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
                x => (x is NebulaCache));

        NebulaCache? castedExecutedMethodAttribute = executedMethodAttribute as NebulaCache;
        return castedExecutedMethodAttribute.CustomCacheName is not null;
    }

    private static string GetCustomCacheName(MethodInfo methodInfo)
    {
        object? executedMethodAttribute = methodInfo.GetCustomAttributes(true)
            .FirstOrDefault(
                x => (x is NebulaCache));

        NebulaCache? castedExecutedMethodAttribute = executedMethodAttribute as NebulaCache;

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
