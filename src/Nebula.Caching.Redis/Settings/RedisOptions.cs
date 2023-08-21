using Common.Settings;

namespace Redis.Settings
{
    public class RedisOptions : BaseOptions
    {
        public string CacheServiceUrl { get; set; } = "";
        public override string ConfigurationRoot { get; set; } = "Redis";
    }
}