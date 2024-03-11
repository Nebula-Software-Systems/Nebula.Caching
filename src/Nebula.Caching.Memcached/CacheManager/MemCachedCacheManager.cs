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

        public async Task<bool> CacheExistsAsync(string key)
        {
            var cache = (await _memCached.GetAsync<string>(key));
            return cache.HasValue;
        }

        public async Task<string> GetAsync(string key)
        {
            return (await _memCached.GetAsync<string>(key)).Value;
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _memCached.SetAsync(key, value, expiration);
        }
    }
}