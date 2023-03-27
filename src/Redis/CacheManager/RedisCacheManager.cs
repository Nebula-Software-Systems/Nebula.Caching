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
            return Get(key) != RedisValue.Null;
        }

        public string Get(string key)
        {
            return _redis.StringGet(key).ToString();
        }

        public void SetAsync(string key, string value, TimeSpan expiration)
        {
            _redis.SetAdd(new RedisKey(key), value);
            _redis.KeyExpire(new RedisKey(key), expiration);
        }
    }
}