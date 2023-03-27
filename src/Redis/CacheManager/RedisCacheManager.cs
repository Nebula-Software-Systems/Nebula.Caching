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

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _redis.SetAddAsync(key, value);
            await _redis.KeyExpireAsync(key, expiration);
        }
    }
}