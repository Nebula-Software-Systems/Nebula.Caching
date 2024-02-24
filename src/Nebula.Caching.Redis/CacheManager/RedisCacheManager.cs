using Nebula.Caching.Common.CacheManager;
using StackExchange.Redis;

namespace Nebula.Caching.Redis.CacheManager
{
    public class RedisCacheManager : ICacheManager
    {
        private IDatabase _redis;

        public RedisCacheManager(IDatabase redis)
        {
            _redis = redis;
        }

        public bool CacheExists(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return _redis.StringGet(key) != RedisValue.Null;
        }

        public string Get(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return _redis.StringGet(key).ToString();
        }

        public void Set(string key, string value, TimeSpan expiration)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(expiration);

            _redis.StringSet(key, value, expiration);
        }

        public async Task<bool> CacheExistsAsync(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            return (await _redis.StringGetAsync(key).ConfigureAwait(false)) != RedisValue.Null;
        }

        public async Task<string> GetAsync(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            return (await _redis.StringGetAsync(key).ConfigureAwait(false)).ToString();
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(expiration);

            await _redis.StringSetAsync(key, value, expiration);
        }
    }
}