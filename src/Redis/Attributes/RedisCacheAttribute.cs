using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Redis.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class RedisCacheAttribute : Attribute
    {
        public int CacheDuration { get; set; } = CacheDurationConstants.DefaultCacheDurationInSeconds;
    }
}