using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redis.Settings;
using StackExchange.Redis;

namespace Common.Settings
{
    public class Configurations
    {
        public string ConfigurationSection { get; set; }
        public RedisConfigurationFlavour ConfigurationFlavour { get; set; }
        public Action<ConfigurationOptions>? Configure { get; set; }
        public ConfigurationOptions? Configuration { get; set; }
        public TextWriter? Log { get; set; }
    }
}