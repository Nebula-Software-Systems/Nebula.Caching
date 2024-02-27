using AspectCore.DynamicProxy;
using Nebula.Caching.Common.Attributes;
using System.Reflection;

namespace Nebula.Caching.Common.Utils
{
    public interface IContextUtils
    {
        Task<int> GetCacheDurationAsync<T>(string key, AspectContext context) where T : BaseAttribute;
        Task<string[]> GetMethodParametersAsync(AspectContext context);
        Task<MethodInfo> GetExecutedMethodInfoAsync(AspectContext context);
        Task<MethodInfo> GetServiceMethodInfoAsync(AspectContext context);
        Task<bool> IsAttributeOfTypeAsync<T>(AspectContext context) where T : BaseAttribute;
    }
}