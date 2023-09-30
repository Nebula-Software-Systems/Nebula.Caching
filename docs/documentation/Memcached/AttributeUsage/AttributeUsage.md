# Using the caching attribute

The attribute that is going to be used is called MemCachedCache.

As of today, we are limited to using this attribute only in interface definition.

An example is shown below.

```csharp

    public interface IMemcachedStuff
    {
        [MemCachedCache]
        List<SomeObject> SomeMethod(int param1, int param2, string name);

        [MemCachedCache]
        Task<List<SomeObject>> AnotherMethod();
    }

```

## Cache Duration

There are several ways to set your cache duration.

### Cache duration from attribute

The easiest way to set the cache duration is by declaring it directly on the interface definition, using the _CacheDurationInSeconds_ property.

```csharp

    public interface IMemcachedStuff
    {
        [MemCachedCache(CacheDurationInSeconds = 120)]
        List<SomeObject> SomeMethod(int param1, int param2, string name);

        [MemCachedCache(CacheDurationInSeconds = 272)]
        Task<List<SomeObject>> AnotherMethod();
    }

```

Keep in mind the _CacheDurationInSeconds_ accepts cache durations in seconds.

If no value is inserted, like the first example, the default cache duration will be applied (which is 600 seconds).

### Cache duration from cache registration

If you want to use another default value for your cache duration, you can do so while registring the cache.

Please check [this](../CacheRegistration/CacheRegistration.md) for more information on how to do so.

### Cache duration from configuration file

In an effort to make our interfaces more clean while still using custom cache duration, you can leave your cache attribute definition without specifying the cache duration, but that same cache duration comes from a configuration section of _appsettings.json_.

When configuring your [cache usage](../CacheRegistration/CacheRegistration.md), you can specify a specific section from your _appsettings.json_ file where you are going to read all your cache configurations from. By default, the section name is **_MemCached_**.

Under the section you defined above, create a new section called _CacheSettings_, where you are going to place the your cache duration.

The key you need to use here should the a template key based upon the namespace your interface implementation lives upon. For more information on what to place on the key part, please refer to [this](../CacheKeyGeneration/CacheKeyGeneration.md).

The value for your cache duration should follow the _HH:MM:SS_ pattern, where HH represents Hours, MM represents Minutes and SS represent seconds. So, for example, if you want your cache to have a duration of 1 hour, 15 minutes and 30 seconds, the value should for the duration should be _01:15:30_.

Below you can find an example of how this could exist in your configuration:

```json
  "MemCached": {
    "CacheSettings": {
      "Gorold-Payment-Attributes-MemcachedStuff--AnotherMethod": "01:15:30",
      "Gorold-Payment-Attributes-MemcachedStuff--SomeMethod--{param1}--{param2}--{name}": "00:20:00"
    }
  }
```

Let's take a closer look at the structure of the second configuration cache key (same conclusions apply for any kind of key to be inserted).

```
//this is the key we are analyzing
Gorold-Payment-Attributes-MemcachedStuff--SomeMethod--{param1}--{param2}--{name}

Gorold-Payment-Attributes-MemcachedStuff -> Namespace where the interface, where the attribute was placed, implementation is located. Notice that we replaced all the '.' for '-'.

--SomeMethod--{param1}--{param2}--{name} -> The remaining part of the key is constitued of the method where the attribute was placed, plus the methods it might contain. If no parameters exist, then we don't need to insert them on the cache key definition. If you notice closely, when you are adding cache duration for methods that take parameters, you must add such parameters inside the curly braces. You should add only the parameter name, not its value. Also notice that, unline above, we use double '-' to separate things.
```

### Hierarchy in cache duration definition

One might ask what happens if I define the cache duration inline, meaning in the interface definition, and also in the _appsettings.json_ file.

As of today, the cache defined in the configuration file will be taken in consideration in this conflict scenario.

So, this are the cache values taken:

1. **_Cache defined both in the configuration file and in the interface method_**: value defined in the configuration file taken
2. **_Cache only defined in the configuration file_**: value defined taken as the cache duration
3. **_Cache only defined in the interface method_**: value defined taken as the cache duration
4. **_Cache not defined in the configuration file nor in the interface method_**: default value for cache duration taken

## Cache Groups

Sometimes you may have the need to give a group of caches the same duration, because there might be some logical grouping that you want to have. For this case, we have created cache groups.

Cache groups allow you to specify, in your attribute definition, the cache group it belongs. That is done via the _CacheGroup_ property. There must be a corresponding configuration section under your cache root configuration section (as we saw before, _MemCached_ was the default for that) that indicates the duration for that cache group; that should be in a section called _CacheGroupSettings_ under your root one. The duration time specificied should follow the pattern spoken about earlier, meaning following the _HH:MM:SS_ pattern

Below you can find an example of cache groups in pratice, which puts the cache created on the method _SomeMethod_ inside a cache group called _GroupA_ and, according to this group's cache configuration, the cache will have a duration of 2 hours 23 minutes and 15 seconds.

### Interface definition of cache group

```csharp

    public interface IMemcachedStuff
    {
        [MemCachedCache(CacheGroup = "GroupA")]
        List<SomeObject> SomeMethod(int param1, int param2, string name);
    }

```

### Configuration definition of cache group

```json
  "MemCached": {
    "CacheGroupSettings": {
      "GroupA" : "02:23:15"
    }
  }
```

## Custom Key Names

Traditionally, you want to use the automatically generated cache keys, because they are designed so that no collisions occur. The way we generate keys is also helpful because they indicate from which method you are caching data from, which can be very helpful.

Despite the advantages of using our generated cache keys, you might want to set your cache keys with custom names which might be better for your, mainly because you might understand them better.

To fulfill that need, we introduced a property in our attribute definition called _**CustomCacheName**_.

An example of that can be seen below:

### Interface definition of custom cache name

```csharp

    public interface IMemcachedStuff
    {
        [MemCachedCache(CustomCacheName = "MyCustomCacheName")]
        List<SomeObject> SomeMethod(int param1, int param2, string name);
    }

```

### Configuration definition of our cache duration based on our custom cache key name

```json
  "MemCached": {
    "CacheGroupSettings": {
      "MyCustomCacheName" : "04:00:00"
    }
  }
```

> :warning: Please note that, if you choose to have a custom cache key name, if you want to use the configurations to specify the cache duration, you need to place the custom cache name chosen, and not the default generated one.

> :warning: When choosing your custom cache names, you must manage key collision, meaning choosing unique custom cache keys, otherwise you will have unwanted behavior in your application.
