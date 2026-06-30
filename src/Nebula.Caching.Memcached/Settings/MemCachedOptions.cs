using Nebula.Caching.Common.Settings;

namespace Nebula.Caching.Memcached.Settings;

public class MemCachedOptions : BaseOptions
{
    public string CacheServiceUrl { get; init; } = "";
    public override string ConfigurationRoot { get; set; } = "MemCached";
}
