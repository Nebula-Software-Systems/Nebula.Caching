using System.Reflection;
using AspectCore.DynamicProxy;
using Nebula.Caching.Common.Attributes;

namespace Nebula.Caching.Common.Utils
{
    public interface IContextUtils
    {
        int GetCacheDuration<T>(string key, AspectContext context) where T : BaseAttribute;
        string[] GetMethodParameters(AspectContext context);
        MethodInfo GetExecutedMethodInfo(AspectContext context);
        MethodInfo GetServiceMethodInfo(AspectContext context);
        bool IsAttributeOfType<T>(AspectContext context) where T : BaseAttribute;
    }
}