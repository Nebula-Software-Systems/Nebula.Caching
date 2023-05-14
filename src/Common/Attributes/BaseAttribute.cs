using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    [ExcludeFromCodeCoverage]
    public class BaseAttribute : Attribute
    {
        public int CacheDurationInSeconds { get; set; } = CacheDurationConstants.DefaultCacheDurationInSeconds;
    }
}