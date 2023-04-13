using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Common.CacheRepresentation.KeyValue
{
    [ExcludeFromCodeCoverage]
    public class CacheKeyValuePairs
    {
        public ConcurrentDictionary<string, TimeSpan> CacheSettings { get; set; }
    }
}