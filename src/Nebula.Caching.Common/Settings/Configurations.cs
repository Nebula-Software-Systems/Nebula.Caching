using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Settings
{
    public class Configurations
    {
        public string ConfigurationSection { get; set; } = CacheConfigurationConstants.ConfigurationSection;
        public int DefaultCacheDurationInSeconds { get; set; } = CacheDurationConstants.DefaultCacheDurationInSeconds;
    }
}