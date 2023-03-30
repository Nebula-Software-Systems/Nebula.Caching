using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Attributes
{
    public class BaseAttribute : Attribute
    {
        public int CacheDuration { get; set; } = CacheDurationConstants.DefaultCacheDurationInSeconds;
    }
}