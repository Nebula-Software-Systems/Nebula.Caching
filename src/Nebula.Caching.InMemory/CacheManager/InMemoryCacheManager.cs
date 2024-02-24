using Microsoft.Extensions.Caching.Memory;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.Common.Compression;
using System.Text;

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

        public async Task<bool> CacheExistsAsync(string key)
        {
            return await Task.Run(() => _memoryCache.TryGetValue(key, out _)).ConfigureAwait(false);
        }

        public async Task<string> GetAsync(string key)
        {
            _memoryCache.TryGetValue(key, out byte[] value);
            var data = await _gzipCompression.DecompressAsync(value);
            return Encoding.UTF8.GetString(data);
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            byte[] compressedData = await _gzipCompression.CompressAsync(Encoding.UTF8.GetBytes(value));

            _memoryCache.Set(key, compressedData, cacheEntryOptions);
        }
    }
}