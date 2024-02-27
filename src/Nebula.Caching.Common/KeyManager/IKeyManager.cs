using System.Reflection;

namespace Nebula.Caching.Common.KeyManager
{
    public interface IKeyManager
    {
        Task<string> GenerateCacheKeyAsync(MethodInfo executedMethodInfo, MethodInfo serviceMethodInfo, string[] parameters);
        Task<string> ConvertCacheKeyToConfigKeyAsync(string key);
    }
}