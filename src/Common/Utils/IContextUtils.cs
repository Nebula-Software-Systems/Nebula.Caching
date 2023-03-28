using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Nebula.Caching.src.Common.Utils
{
    public interface IContextUtils
    {
        int GetCacheDuration(AspectContext context);
        string[] GetMethodParameters(AspectContext context);
        MethodInfo GetExecutedMethodInfo(AspectContext context);
    }
}