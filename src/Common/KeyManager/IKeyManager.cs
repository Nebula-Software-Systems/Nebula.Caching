using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nebula.Caching.Common.KeyManager
{
    public interface IKeyManager
    {
        string GenerateKey(MethodInfo methodInfo, string[] parameters);
        string ConvertCacheKeyToConfigKey(string key);
    }
}