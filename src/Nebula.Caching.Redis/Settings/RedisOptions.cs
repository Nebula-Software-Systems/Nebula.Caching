using Common.Settings;

namespace Redis.Settings
{
    public class RedisOptions : BaseOptions
    {
        public string CacheServiceUrl { get; init; } = "";
        public override string ConfigurationRoot { get; set; } = "Redis";
    }
}