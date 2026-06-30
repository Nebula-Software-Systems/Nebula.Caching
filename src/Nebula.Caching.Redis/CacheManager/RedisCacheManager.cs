using Nebula.Caching.Common.CacheManager;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.CacheManager;

public class RedisCacheManager(IDatabase redis) : ICacheManager
{
    public bool CacheExists(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return redis.StringGet(key) != RedisValue.Null;
    }

    public string Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return redis.StringGet(key).ToString();
    }

    public void Set(string key, string value, TimeSpan expiration)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        redis.StringSet(key, value, expiration);
    }

    public async Task<bool> CacheExistsAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return (await redis.StringGetAsync(key).ConfigureAwait(false)) != RedisValue.Null;
    }

    public async Task<string> GetAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return (await redis.StringGetAsync(key).ConfigureAwait(false)).ToString();
    }

    public Task SetAsync(string key, string value, TimeSpan expiration)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        return redis.StringSetAsync(key, value, expiration);
    }
}