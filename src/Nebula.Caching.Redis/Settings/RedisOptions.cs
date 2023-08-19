using Common.Settings;
using System.Diagnostics.CodeAnalysis;

namespace Redis.Settings
{
    [ExcludeFromCodeCoverage]
    public class RedisOptions : BaseOptions
    {
        public string CacheServiceUrl { get; set; } = "";
        public override string ConfigurationRoot { get; set; } = "Redis";
    }
}