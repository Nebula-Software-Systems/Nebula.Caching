
using Nebula.Caching.InMemory.Attributes;

namespace Nebula.Caching.InMemorySample.Interfaces
{
    public interface IService
    {
        //this method will use the cache duration from settings.json config file
        [InMemoryCache]
        string OneMethod(string name, int year);
    
        //this method will use the cache duration defined in the settings.json config file, not the default cache duration
        //uses custom name
        [InMemoryCache(CustomCacheName = "MagicName")]
        int MagicMethod();
    
        //this method will use the cache duration defined on the constructor
        [InMemoryCache(CacheDurationInSeconds = 120)]
        ComplexObject AnotherMethod(string someParameter);

        //this method will use the cache duration defined for this cache group on our settings.json config file
        [InMemoryCache(CacheGroup = "GroupA")]
        bool SomeMethod();
    }

    public class ComplexObject
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}