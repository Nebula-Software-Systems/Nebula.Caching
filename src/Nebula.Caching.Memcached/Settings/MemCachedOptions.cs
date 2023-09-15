using Common.Settings;

namespace Nebula.Caching.MemCached.Settings
{
    public class MemCachedOptions : BaseOptions
    {
        public string CacheServiceUrl { get; init; } = "";
        public override string ConfigurationRoot { get; set; } = "MemCached";
    }
}