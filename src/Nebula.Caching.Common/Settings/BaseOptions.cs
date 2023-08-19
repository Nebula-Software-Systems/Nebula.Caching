using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Common.Settings
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseOptions
    {
        public virtual string ConfigurationRoot { get; set; } = "";
        public virtual Dictionary<string, TimeSpan> CacheSettings { get; set; } = new();
        public virtual Dictionary<string, TimeSpan> CacheGroupSettings { get; set; } = new();
    }
}