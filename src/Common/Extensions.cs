using AspectCore.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

namespace Nebula.Caching.Common
{
    public static class Extensions
    {
        public static IHostBuilder UseNebulaCaching(this IHostBuilder builderHost)
        {
            builderHost.UseDynamicProxy();
            return builderHost;
        }
    }
}