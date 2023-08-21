using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Common.Settings
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseOptions
    {
        public virtual string ConfigurationRoot { get; set; } = "";
        public virtual IDictionary<string, TimeSpan> CacheSettings { get; set; } = new ConcurrentDictionary<string, TimeSpan>();
        public virtual IDictionary<string, TimeSpan> CacheGroupSettings { get; set; } = new ConcurrentDictionary<string, TimeSpan>();
    }
}