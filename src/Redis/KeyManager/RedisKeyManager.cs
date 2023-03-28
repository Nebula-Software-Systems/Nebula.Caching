using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nebula.Caching.Common.KeyManager;

namespace Nebula.Caching.Redis.KeyManager
{
    public class RedisKeyManager : IKeyManager
    {

        public RedisKeyManager()
        {

        }

        public string GenerateKey(MethodInfo methodInfo, string[] parameters)
        {
            string methodParamsAggregated = string.Join(":", parameters);
            return $"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}:{methodParamsAggregated}";
        }
    }
}