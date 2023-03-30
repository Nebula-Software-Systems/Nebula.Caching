using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Nebula.Caching.Common.Attributes;
using Nebula.Caching.Redis.Attributes;

namespace Nebula.Caching.Common.Utils
{
    public interface IContextUtils
    {
        int GetCacheDuration<T>(AspectContext context) where T : BaseAttribute;
        string[] GetMethodParameters(AspectContext context);
        MethodInfo GetExecutedMethodInfo(AspectContext context);
        bool IsAttributeOfType<T>(AspectContext context) where T : BaseAttribute;
    }
}