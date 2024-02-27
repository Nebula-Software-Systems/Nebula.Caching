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
        private CacheBaseOptions _baseOptions;

        public ContextUtils(IKeyManager keyManager, CacheBaseOptions baseOptions)
        {
            _keyManager = keyManager;
            _baseOptions = baseOptions;
        }

        public async Task<int> GetCacheDurationAsync<T>(string key, AspectContext context) where T : BaseAttribute
        {
            return await CacheExistInConfigurationAsync(key, context) ?
                                 await RetrieveCacheExpirationFromConfigAsync(key, context)
                                :
                                 await RetrieveCacheExpirationFromAttributeAsync<T>(context);
        }

        public Task<string[]> GetMethodParametersAsync(AspectContext context)
        {
            return Task.Run(() =>
            {
                var methodParams = context.Parameters.Select((object obj) =>
                {
                    return obj.ToString();
                }).ToArray();
                return methodParams;
            });
        }

        public Task<bool> IsAttributeOfTypeAsync<T>(AspectContext context) where T : BaseAttribute
        {
            return Task.Run(() =>
            {
                var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                .FirstOrDefault(
                                                x => typeof(T).IsAssignableFrom(x.GetType())
                                                );
                return executedMethodAttribute is T;
            });
        }

        public Task<MethodInfo> GetExecutedMethodInfoAsync(AspectContext context)
        {
            return Task.Run(() => context.ImplementationMethod);
        }

        public Task<MethodInfo> GetServiceMethodInfoAsync(AspectContext context)
        {
            return Task.Run(() => context.ServiceMethod);
        }

        private async Task<bool> CacheExistInConfigurationAsync(string key, AspectContext context)
        {
            if (_baseOptions.CacheSettings is null) return false;

            var convertedKey = await _keyManager.ConvertCacheKeyToConfigKeyAsync(await _keyManager.GenerateCacheKeyAsync(context.ImplementationMethod, context.ServiceMethod, await GenerateParamsFromParamCollectionAsync(context.GetParameters())));
            return _baseOptions.CacheSettings.ContainsKey(convertedKey);
        }


        private async Task<int> RetrieveCacheExpirationFromConfigAsync(string key, AspectContext context)
        {
            ArgumentNullException.ThrowIfNull(key);

            var convertedKey = await _keyManager.ConvertCacheKeyToConfigKeyAsync(await _keyManager.GenerateCacheKeyAsync(context.ImplementationMethod, context.ServiceMethod, await GenerateParamsFromParamCollectionAsync(context.GetParameters())));
            _baseOptions.CacheSettings.TryGetValue(convertedKey, out TimeSpan cacheExpiration);

            if (await IsCacheExpirationValidAsync(cacheExpiration))
            {
                return (int)cacheExpiration.TotalSeconds;
            }

            throw new InvalidOperationException($"Cache key {key} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
        }

        private async Task<int> RetrieveCacheExpirationFromAttributeAsync<T>(AspectContext context) where T : BaseAttribute
        {
            var executedMethodAttribute = context.ServiceMethod.GetCustomAttributes(true)
                                                .FirstOrDefault(
                                                                    x => typeof(T).IsAssignableFrom(x.GetType())
                                                                );

            var castedExecutedMethodAttribute = executedMethodAttribute as T;

            return await IsCacheGroupDefinedAsync(castedExecutedMethodAttribute) ? await RetrieveCacheExpirationFromCacheGroupAsync(castedExecutedMethodAttribute.CacheGroup)
                                                                                   :
                                                                                   castedExecutedMethodAttribute.CacheDurationInSeconds;
        }

        private Task<bool> IsCacheGroupDefinedAsync(BaseAttribute attribute)
        {
            return Task.Run(() => !string.IsNullOrEmpty(attribute.CacheGroup));
        }

        private async Task<int> RetrieveCacheExpirationFromCacheGroupAsync(string cacheGroup)
        {
            _baseOptions.CacheGroupSettings.TryGetValue(cacheGroup, out TimeSpan cacheExpiration);

            if (await IsCacheExpirationValidAsync(cacheExpiration))
            {
                return (int)cacheExpiration.TotalSeconds;
            }

            throw new InvalidOperationException($"Cache group {cacheGroup} either doesn't exist on the configuration or if exist has an invalid value for its duration. Cache duration should be greater than zero.");
        }

        private Task<bool> IsCacheExpirationValidAsync(TimeSpan? cacheExpiration)
        {
            return Task.Run(() => cacheExpiration != null && cacheExpiration > TimeSpan.Zero);
        }

        private async Task<string[]> GenerateParamsFromParamCollectionAsync(ParameterCollection parameters)
        {
            List<string> genericParamsList = new List<string>();

            foreach (var param in parameters)
            {
                var genericParam = await GenerateGenericConfigCacheParameterAsync(param.Name);
                genericParamsList.Add(genericParam);
            }

            return genericParamsList.ToArray();
        }

        private Task<string> GenerateGenericConfigCacheParameterAsync(string parameter)
        {
            return Task.Run(() =>
            {
                ArgumentNullException.ThrowIfNull(parameter);
                return $"{{{parameter}}}";
            });
        }
    }
}