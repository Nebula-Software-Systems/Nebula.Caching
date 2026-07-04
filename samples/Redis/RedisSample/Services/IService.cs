using Nebula.Caching.Common.Attributes;

namespace RedisSample.Services;

public interface IService
{
    [NebulaCache]
    Task<string> HelloWorldAsync();

    [NebulaCache(CustomCacheName = "IService-HelloWorld", CacheDurationInSeconds = 3600)]
    string HelloWorld(string name);

    [NebulaCache(CacheGroup = "Calculator")]
    int SumConfigCache(int num1, int num2);
}
