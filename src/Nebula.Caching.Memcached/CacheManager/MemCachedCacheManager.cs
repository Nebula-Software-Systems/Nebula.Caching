using Enyim.Caching;
using Enyim.Caching.Memcached.Results;
using Nebula.Caching.Common.CacheManager;

namespace Nebula.Caching.Memcached.CacheManager;

public class MemCachedCacheManager(IMemcachedClient memCached) : ICacheManager
{
    public bool CacheExists(string key)
    {
        return memCached.Get<object>(key) is not null;
    }

    public async Task<bool> CacheExistsAsync(string key)
    {
        IGetOperationResult<string>? cache = (await memCached.GetAsync<string>(key).ConfigureAwait(false));
        return cache.HasValue;
    }

    public string Get(string key)
    {
        return memCached.Get<string>(key);
    }

    public async Task<string> GetAsync(string key)
    {
        return (await memCached.GetAsync<string>(key).ConfigureAwait(false)).Value;
    }

    public void Set(string key, string value, TimeSpan expiration)
    {
        memCached.Set(key, value, expiration);
    }

    public Task SetAsync(string key, string value, TimeSpan expiration)
    {
        return memCached.SetAsync(key, value, expiration);
    }
}