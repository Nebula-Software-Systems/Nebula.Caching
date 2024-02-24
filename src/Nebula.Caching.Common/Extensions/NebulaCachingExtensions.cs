using AspectCore.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Nebula.Caching.Common.Extensions
{
    public static class NebulaCachingExtensions
    {
        public static IHostBuilder UseNebulaCaching(this IHostBuilder builderHost)
        {
            return builderHost.UseDynamicProxy();
        }
    }
}