using Enyim.Caching;
using Nebula.Caching.Common.CacheManager;

namespace Nebula.Caching.Memcached.CacheManager
{
    public class MemCachedCacheManager : ICacheManager
    {
        private readonly IMemcachedClient _memCached;

        public MemCachedCacheManager(IMemcachedClient memCached)
        {
            _memCached = memCached;
        }

        public bool CacheExists(string key)
        {
            return _memCached.Get<object>(key) is not null;
        }

        public async Task<bool> CacheExistsAsync(string key)
        {
            var cache = (await _memCached.GetAsync<string>(key));
            return cache.HasValue;
        }

        public string Get(string key)
        {
            return _memCached.Get<string>(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return (await _memCached.GetAsync<string>(key)).Value;
        }

        public void Set(string key, string value, TimeSpan expiration)
        {
            _memCached.Set(key, value, expiration);
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _memCached.SetAsync(key, value, expiration);
        }
    }
}