using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Common.CacheRepresentation.KeyValue;
using Common.Settings;

namespace Redis.Settings
{
    [ExcludeFromCodeCoverage]
    public class RedisOptions : BaseOptions
    {
        public override string ConfigurationRoot { get; set; } = "Redis";
    }
}