using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Common.Settings;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.KeyManager;
using System.Reflection;

namespace Nebula.Caching.Common.Utils
{
    public class ContextUtils : IContextUtils
    {

        private IKeyManager _keyManager;
        private BaseOptions _baseOptions;

        public ContextUtils(IKeyManager keyManager, BaseOptions baseOptions)
        {
            _keyManager = keyManager;
            _baseOptions = baseOptions;
        }

        public int GetCacheDuration<T>(string key, AspectContext context) where T : BaseAttribute
        {
            return CacheExistInConfiguration(key, context) ?
                                                RetrieveCacheExpirationFromConfig(key, context)
                                                :
                                                RetrieveCacheExpirationFromAttribute<T>(context);
        }

        public bool CacheExistInConfiguration(string key, AspectContext context)
        {
            if (_baseOptions.CacheSettings is null) return false;

            var convertedKey = _keyManager.ConvertCacheKeyToConfigKey(_keyManager.GenerateKey(context.ImplementationMethod, context.ServiceMethod, GenerateParamsFromParamCollection(context.GetParameters())));
            return _baseOptions.CacheSettings.ContainsKey(convertedKey);
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

        public bool CacheConfigSectionExists()
        {
            return _baseOptions.CacheSettings != null;
        }

        public int RetrieveCacheExpirationFromConfig(string key, AspectContext context)
        {
            ArgumentNullException.ThrowIfNull(key);

            var convertedKey = _keyManager.ConvertCacheKeyToConfigKey(_keyManager.GenerateKey(context.ImplementationMethod, context.ServiceMethod, GenerateParamsFromParamCollection(context.GetParameters())));
            _baseOptions.CacheSettings.TryGetValue(convertedKey, out TimeSpan cacheExpiration);

            if (IsCacheExpirationValid(cacheExpiration))
            {
                return (int)cacheExpiration.TotalSeconds;
            }

            throw new InvalidOperationException($"Cache key {key} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
        }

        public int RetrieveCacheExpirationFromAttribute<T>(AspectContext context) where T : BaseAttribute
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                .FirstOrDefault(
                                                                    x => typeof(T).IsAssignableFrom(x.GetType())
                                                                );

            var castedExecutedMethodAttribute = executedMethodAttribute as T;

            return IsCacheGroupDefined(castedExecutedMethodAttribute) ?
                                                                                    RetrieveCacheExpirationFromCacheGroup(castedExecutedMethodAttribute.CacheGroup)
                                                                                    :
                                                                                    castedExecutedMethodAttribute.CacheDurationInSeconds;
        }

        public bool IsCacheGroupDefined(BaseAttribute attribute)
        {
            return !String.IsNullOrEmpty(attribute.CacheGroup);
        }

        public int RetrieveCacheExpirationFromCacheGroup(string cacheGroup)
        {
            _baseOptions.CacheGroupSettings.TryGetValue(cacheGroup, out TimeSpan cacheExpiration);

            if (IsCacheExpirationValid(cacheExpiration))
            {
                return (int)cacheExpiration.TotalSeconds;
            }

            throw new InvalidOperationException($"Cache group {cacheGroup} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
        }

        public bool IsCacheExpirationValid(TimeSpan? cacheExpiration)
        {
            return cacheExpiration != null && cacheExpiration > TimeSpan.Zero;
        }

        public string[] GenerateParamsFromParamCollection(ParameterCollection parameters)
        {
            List<string> genericParamsList = new List<string>();

            foreach (var param in parameters)
            {
                var genericParam = GenerateGeneriConfigCacheParameter(param.Name);
                genericParamsList.Add(genericParam);
            }

            return genericParamsList.ToArray();
        }

        public string GenerateGeneriConfigCacheParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);
            return $"{{{parameter}}}";
        }

    }
}