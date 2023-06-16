using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Nebula.Caching.Common.CacheManager;

namespace Nebula.Caching.InMemory.CacheManager
{
    public class InMemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool CacheExists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public async Task<bool> CacheExistsAsync(string key)
        {
            return await Task.Run(() => CacheExists(key)).ConfigureAwait(false);
        }

        public string Get(string key)
        {
            _memoryCache.TryGetValue(key, out string value);
            return value;
        }

        public async Task<string> GetAsync(string key)
        {
            return await Task.Run(() => Get(key)).ConfigureAwait(false);
        }

        public void Set(string key, string value, TimeSpan expiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await Task.Run(() => Set(key, value, expiration)).ConfigureAwait(false);
        }
    }
}