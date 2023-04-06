using System.Collections.Concurrent;
using Common.CacheRepresentation.KeyValue;
using Common.Settings;

namespace Redis.Settings
{
    public class RedisOptions : BaseOptions
    {
        public override string ConfigurationRoot { get; set; } = "Redis";
    }
}