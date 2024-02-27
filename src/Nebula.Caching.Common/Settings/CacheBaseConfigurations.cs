using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Settings
{
    public abstract class CacheBaseConfigurations
    {
        public virtual string ConfigurationSection { get; init; } = CacheConstants.DefaultConfigurationSection;
        public int DefaultCacheDurationInSeconds { get; init; } = CacheConstants.DefaultCacheDurationInSeconds;
    }
}