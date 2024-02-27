using Common.Settings;

namespace Redis.Settings
{
    public class RedisOptions : CacheBaseOptions
    {
        public string CacheServiceUrl { get; init; } = string.Empty;
    }
}