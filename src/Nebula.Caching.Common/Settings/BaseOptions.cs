using System.Collections.Concurrent;

namespace Common.Settings
{
    public abstract class BaseOptions
    {
        public virtual string ConfigurationRoot { get; set; } = "";
        public virtual IDictionary<string, TimeSpan> CacheSettings { get; init; } = new ConcurrentDictionary<string, TimeSpan>();
        public virtual IDictionary<string, TimeSpan> CacheGroupSettings { get; init; } = new ConcurrentDictionary<string, TimeSpan>();
    }
}