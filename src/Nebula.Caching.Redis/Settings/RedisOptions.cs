using Common.Settings;

namespace Redis.Settings
{
    public class RedisOptions : BaseOptions
    {
        public string CacheServiceUrl { get; init; } = string.Empty;
    }
}