using Common.Settings;

namespace Nebula.Caching.MemCached.Settings
{
    public class MemCachedOptions : CacheBaseOptions
    {
        public string CacheServiceUrl { get; init; } = string.Empty;
    }
}