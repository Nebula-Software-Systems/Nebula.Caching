using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            return _redis.StringGet(key) != RedisValue.Null;
        }

        public string Get(string key)
        {
            return _redis.StringGet(key).ToString();
        }

        public void Set(string key, string value, TimeSpan expiration)
        {
            _redis.StringSet(key, value, expiration);
        }

        public async Task<bool> CacheExistsAsync(string key)
        {
            return (await _redis.StringGetAsync(key).ConfigureAwait(false)) != RedisValue.Null;
        }

        public async Task<string> GetAsync(string key)
        {
            return (await _redis.StringGetAsync(key).ConfigureAwait(false)).ToString();
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _redis.StringSetAsync(key, value, expiration);
        }
    }
}