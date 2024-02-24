using Common.Settings;

namespace Nebula.Caching.MemCached.Settings
{
    public class MemCachedOptions : BaseOptions
    {
        public string CacheServiceUrl { get; init; } = string.Empty;
    }
}