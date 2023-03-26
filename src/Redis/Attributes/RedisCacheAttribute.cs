namespace Nebula.Caching.Redis.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class RedisCacheAttribute : Attribute
    {
        public int CacheDuration { get; set; }
    }
}