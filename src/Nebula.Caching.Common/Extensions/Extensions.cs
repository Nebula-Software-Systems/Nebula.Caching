using System.Diagnostics.CodeAnalysis;
using AspectCore.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Nebula.Caching.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IHostBuilder UseNebulaCaching(this IHostBuilder builderHost)
        {
            return builderHost.UseDynamicProxy();
        }
    }
}