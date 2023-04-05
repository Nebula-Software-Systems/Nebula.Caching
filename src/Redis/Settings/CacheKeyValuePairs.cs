using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.Settings
{
    public class CacheKeyValuePairs
    {
        public ConcurrentDictionary<string, TimeSpan> CacheSettings { get; set; }
    }
}