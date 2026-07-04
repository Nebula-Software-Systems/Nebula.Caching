using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Settings;

public sealed class BaseOptions
{
    public string ConfigurationRoot { get; init; } = CacheConstants.DefaultConfigurationRoot;
    public int DefaultCacheDurationInSeconds { get; init; } = CacheConstants.DefaultCacheDurationInSeconds;
    public IDictionary<string, TimeSpan>? CacheSettings { get; init; }
    public IDictionary<string, TimeSpan>? CacheGroupSettings { get; init; }
}
