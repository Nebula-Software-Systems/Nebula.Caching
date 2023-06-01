using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Nebula.Caching.Common.CacheManager
{
    public interface ICacheManager
    {
        void Set(string key, string value, TimeSpan expiration);
        string Get(string key);
        bool CacheExists(string key);
        Task SetAsync(string key, string value, TimeSpan expiration);
        Task<string> GetAsync(string key);
        Task<bool> CacheExistsAsync(string key);
    }
}