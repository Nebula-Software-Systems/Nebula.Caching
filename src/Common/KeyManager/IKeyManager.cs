using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Nebula.Caching.Common.KeyManager
{
    public interface IKeyManager
    {
        string GenerateKey(MethodInfo methodInfo, string[] parameters);
        string ConvertCacheKeyToConfigKey(string key);
    }
}