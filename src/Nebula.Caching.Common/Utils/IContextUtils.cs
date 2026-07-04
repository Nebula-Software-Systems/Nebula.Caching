using System.Reflection;
using AspectCore.DynamicProxy;

namespace Nebula.Caching.Common.Utils;

public interface IContextUtils
{
    int GetCacheDuration(string key, AspectContext context);
    string[] GetMethodParameters(AspectContext context);
    MethodInfo GetExecutedMethodInfo(AspectContext context);
    MethodInfo GetServiceMethodInfo(AspectContext context);
    bool IsAttributeOfType(AspectContext context);
}
