using System.Collections.Generic;
using System.Reflection;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.KeyManager;

namespace Nebula.Caching.Common.Utils
{
    public class ContextUtils : IContextUtils
    {

        private IKeyManager _keyManager;
        private IConfiguration _configuration;
        private BaseOptions _baseOptions;

        public ContextUtils(IKeyManager keyManager, IConfiguration configuration, BaseOptions baseOptions)
        {
            _keyManager = keyManager;
            _configuration = configuration;
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
            return _baseOptions.CacheSettings.ContainsKey(key);
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

        public bool CacheConfigSectionExists()
        {
            return _baseOptions.CacheSettings != null;
        }

        public int RetrieveCacheExpirationFromConfig(string key, AspectContext context)
        {
            ArgumentNullException.ThrowIfNull(key);

            var convertedKey = _keyManager.ConvertCacheKeyToConfigKey(_keyManager.GenerateKey(context.ImplementationMethod, GenerateParamsFromParamCollection(context.GetParameters())));
            var cacheExpiration = _baseOptions.CacheSettings.GetValueOrDefault(convertedKey);

            if (IsCacheExpirationValid(cacheExpiration))
            {
                return (int)cacheExpiration.TotalSeconds;
            }

            throw new InvalidOperationException($"Cache key {key} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be grater than zero.");
        }

        public int RetrieveCacheExpirationFromAttribute<T>(AspectContext context) where T : BaseAttribute
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                .FirstOrDefault(
                                                                    x => typeof(T).IsAssignableFrom(x.GetType())
                                                                );
            var castedExecutedMethodAttribute = executedMethodAttribute as T;
            return castedExecutedMethodAttribute.CacheDurationInSeconds;
        }

        public bool MethoExecutedHasCacheGroup<T>(AspectContext context) where T : BaseAttribute
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                    .FirstOrDefault(
                                                        x => typeof(T).IsAssignableFrom(x.GetType())
                                                    );
            var castedExecutedMethodAttribute = executedMethodAttribute as T;
            return !String.IsNullOrEmpty(castedExecutedMethodAttribute.CacheGroup);
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