using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Common.Settings
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseOptions
    {
        public virtual string ConfigurationRoot { get; set; }
        public virtual string CacheServiceUrl { get; set; }
        public virtual ConcurrentDictionary<string, TimeSpan> CacheSettings { get; set; }
    }
}