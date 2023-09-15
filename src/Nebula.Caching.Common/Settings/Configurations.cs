using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Settings
{
    public class Configurations
    {
        public string ConfigurationSection { get; init; } = CacheConfigurationConstants.ConfigurationSection;
        public int DefaultCacheDurationInSeconds { get; init; } = CacheDurationConstants.DefaultCacheDurationInSeconds;
    }
}