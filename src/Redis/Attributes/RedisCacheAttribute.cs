using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Redis.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class RedisCacheAttribute : BaseAttribute
    {
    }
}