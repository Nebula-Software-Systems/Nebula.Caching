using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Nebula.Caching.Common.CacheManager;
using Nebula.Caching.InMemory.Compression;

namespace Nebula.Caching.InMemory.CacheManager;

public class InMemoryCacheManager(IMemoryCache memoryCache) : ICacheManager
{
    public bool CacheExists(string key)
    {
        return memoryCache.TryGetValue(key, out _);
    }

    public Task<bool> CacheExistsAsync(string key)
    {
        return Task.Run(() => CacheExists(key));
    }

    public string Get(string key)
    {
        memoryCache.TryGetValue(key, out byte[] value);
        byte[] data = GZipCompression.Decompress(value);
        return Encoding.UTF8.GetString(data);
    }

    public Task<string> GetAsync(string key)
    {
        return Task.Run(() => Get(key));
    }

    public void Set(string key, string value, TimeSpan expiration)
    {
        MemoryCacheEntryOptions cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        byte[] compressedData = GZipCompression.Compress(Encoding.UTF8.GetBytes(value));

        memoryCache.Set(key, compressedData, cacheEntryOptions);
    }

    public Task SetAsync(string key, string value, TimeSpan expiration)
    {
        return Task.Run(() => Set(key, value, expiration));
    }
}
