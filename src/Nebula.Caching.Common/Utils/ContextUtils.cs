using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.KeyManager;
using System.Reflection;
using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Common.Utils;

public class ContextUtils(IKeyManager keyManager, BaseOptions baseOptions) : IContextUtils
{
    public int GetCacheDuration<T>(string key, AspectContext context) where T : BaseAttribute
    {
        return CacheExistInConfiguration(key, context) ?
            RetrieveCacheExpirationFromConfig(key, context)
            :
            RetrieveCacheExpirationFromAttribute<T>(context);
    }

    public bool CacheExistInConfiguration(string key, AspectContext context)
    {
        if (baseOptions.CacheSettings is null) return false;

        string convertedKey = keyManager.ConvertCacheKeyToConfigKey(keyManager.GenerateKey(context.ImplementationMethod, context.ServiceMethod, GenerateParamsFromParamCollection(context.GetParameters())));
        return baseOptions.CacheSettings.ContainsKey(convertedKey);
    }

    public MethodInfo GetExecutedMethodInfo(AspectContext context)
    {
        return context.ImplementationMethod;
    }

    public MethodInfo GetServiceMethodInfo(AspectContext context)
    {
        return context.ServiceMethod;
    }

    public string[] GetMethodParameters(AspectContext context)
    {
        string?[] methodParams = context.Parameters.Select((object obj) =>
        {
            return obj.ToString();
        }).ToArray();
        return methodParams;
    }

    public bool IsAttributeOfType<T>(AspectContext context) where T : BaseAttribute
    {
        object? executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
            .FirstOrDefault(
                x => (x is T));
        return executedMethodAttribute is T;
    }

    public bool CacheConfigSectionExists()
    {
        return baseOptions.CacheSettings != null;
    }

    public int RetrieveCacheExpirationFromConfig(string key, AspectContext context)
    {
        ArgumentNullException.ThrowIfNull(key);

        string convertedKey = keyManager.ConvertCacheKeyToConfigKey(keyManager.GenerateKey(context.ImplementationMethod, context.ServiceMethod, GenerateParamsFromParamCollection(context.GetParameters())));
        baseOptions.CacheSettings.TryGetValue(convertedKey, out TimeSpan cacheExpiration);

        if (IsCacheExpirationValid(cacheExpiration))
        {
            return (int)cacheExpiration.TotalSeconds;
        }

        throw new InvalidOperationException($"Cache key {key} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
    }

    public int RetrieveCacheExpirationFromAttribute<T>(AspectContext context) where T : BaseAttribute
    {
        object? executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
            .FirstOrDefault(
                x => x is T
            );

        var castedExecutedMethodAttribute = executedMethodAttribute as T;

        return IsCacheGroupDefined(castedExecutedMethodAttribute) ?
            RetrieveCacheExpirationFromCacheGroup(castedExecutedMethodAttribute.CacheGroup)
            :
            castedExecutedMethodAttribute.CacheDurationInSeconds;
    }

    private static bool IsCacheGroupDefined(BaseAttribute attribute)
    {
        return !string.IsNullOrEmpty(attribute.CacheGroup);
    }

    private int RetrieveCacheExpirationFromCacheGroup(string cacheGroup)
    {
        baseOptions.CacheGroupSettings.TryGetValue(cacheGroup, out TimeSpan cacheExpiration);

        if (IsCacheExpirationValid(cacheExpiration))
        {
            return (int)cacheExpiration.TotalSeconds;
        }

        throw new InvalidOperationException($"Cache group {cacheGroup} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
    }

    private static bool IsCacheExpirationValid(TimeSpan? cacheExpiration)
    {
        return cacheExpiration != null && cacheExpiration > TimeSpan.Zero;
    }

    private static string[] GenerateParamsFromParamCollection(ParameterCollection parameters)
    {
        List<string> genericParamsList = new List<string>();

        foreach (var param in parameters)
        {
            string genericParam = GenerateGeneriConfigCacheParameter(param.Name);
            genericParamsList.Add(genericParam);
        }

        return genericParamsList.ToArray();
    }

    private static string GenerateGeneriConfigCacheParameter(string parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        return $"{{{parameter}}}";
    }
}
