using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.Compression;

namespace Nebula.Caching.InMemory.CacheManager
{
    public class InMemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly GZipCompression _gzipCompression;

        public InMemoryCacheManager(IMemoryCache memoryCache, GZipCompression gzipCompression)
        {
            _memoryCache = memoryCache;
            _gzipCompression = gzipCompression;
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
            _memoryCache.TryGetValue(key, out byte[] value);
            var data = _gzipCompression.Decompress(value);
            return Encoding.UTF8.GetString(data);
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

            byte[] compressedData = _gzipCompression.Compress(Encoding.UTF8.GetBytes(value));

            _memoryCache.Set(key, compressedData, cacheEntryOptions);
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await Task.Run(() => Set(key, value, expiration)).ConfigureAwait(false);
        }
    }
}