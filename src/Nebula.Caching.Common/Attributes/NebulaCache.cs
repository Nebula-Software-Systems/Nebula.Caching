using Nebula.Caching.Common.Constants;

namespace Nebula.Caching.Common.Attributes;

/// <summary>
/// Attribute that you would decorate your methods with.
/// Having this attribute will enable caching in the decorated method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class NebulaCache : Attribute
{
    /// <summary>
    /// How long the value would be cached for, in seconds.
    /// If no value is provided, a default value will be used.
    /// </summary>
    public int CacheDurationInSeconds { get; init; } = CacheConstants.DefaultCacheDurationInSeconds;

    /// <summary>
    ///
    /// </summary>
    public string? CacheGroup { get; init; }

    /// <summary>
    ///
    /// </summary>
    public string? CustomCacheName { get; init; }
}
