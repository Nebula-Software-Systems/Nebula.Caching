using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Nebula.Caching.Common.Constants
{
    [ExcludeFromCodeCoverage]
    public class KeyConstants
    {
        public const char MethodFullPathSeparator = '.';
        public const string MethodAndParametersSeparator = ":";
        public const char ConfigMethodFullPathSeparator = '-';
        public const string ConfigMethodAndParametersSeparator = "--";

    }

}