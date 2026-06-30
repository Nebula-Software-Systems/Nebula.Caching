using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Redis.Settings;

public class RedisOptions : BaseOptions
{
    public string CacheServiceUrl { get; init; } = "";
    public override string ConfigurationRoot { get; set; } = "Redis";
}
