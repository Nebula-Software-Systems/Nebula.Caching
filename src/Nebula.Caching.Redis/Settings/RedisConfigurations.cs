using Nebula.Caching.Common.Settings;
using Redis.Settings;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.Settings
{
    public class RedisConfigurations : CacheBaseConfigurations
    {
        public RedisConfigurationFlavour? ConfigurationFlavour { get; init; } = RedisConfigurationFlavour.Vanilla;
        public Action<ConfigurationOptions>? Configure { get; set; }
        public ConfigurationOptions? Configuration { get; set; }
        public TextWriter? Log { get; set; }
    }
}