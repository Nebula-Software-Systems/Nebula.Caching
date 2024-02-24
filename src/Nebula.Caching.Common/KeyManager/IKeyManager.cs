using System.Reflection;

namespace Nebula.Caching.Common.KeyManager
{
    public interface IKeyManager
    {
        string GenerateKey(MethodInfo executedMethodInfo, MethodInfo serviceMethodInfo, string[] parameters);
        string ConvertCacheKeyToConfigKey(string key);
    }
}