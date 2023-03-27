using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Nebula.Caching.Common.CacheManager
{
    public interface ICacheManager
    {
        Task SetAsync(string key, string value, TimeSpan expiration);
    }
}