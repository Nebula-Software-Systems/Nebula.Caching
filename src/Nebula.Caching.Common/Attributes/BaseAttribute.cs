using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class BaseAttribute : Attribute
    {
        public int CacheDurationInSeconds { get; set; } = CacheConstants.DefaultCacheDurationInSeconds;
        public string? CacheGroup { get; set; }
        public string? CustomCacheName { get; set; }
    }
}