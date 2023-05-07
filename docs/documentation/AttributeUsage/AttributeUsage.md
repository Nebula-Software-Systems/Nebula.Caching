# Using the caching attribute

The attribute that is going to be used is called RedisCache.

As of today, we are limited to using this attribute only in interface definition.

An example is shown below.

```csharp

    public interface IRedisStuff
    {
        [RedisCache]
        List<SomeObject> SomeMethod(int param1, int param2);

        [RedisCache]
        Task<List<SomeObject>> AnotherMethod();
    }

```

## Cache Duration

There are several ways to set your cache duration.

### Cache duration from attribute

The easiest way to set the cache duration is by declaring it directly on the interface definition, using the _CacheDuration_ property.

```csharp

    public interface IRedisStuff
    {
        [RedisCache(CacheDuration = 120)]
        List<SomeObject> SomeMethod(int param1, int param2);

        [RedisCache(CacheDuration = 272)]
        Task<List<SomeObject>> AnotherMethod();
    }

```

Keep in mind the _CacheDuration_ accepts cache durations in seconds.

If no value is inserted, like the first example, the default cache duration will be applied (which is 30 seconds).

### Cache duration from cache registration

If you want to use another default value for your cache duration, you can do so while registring the cache.

Please check [this](../CacheRegistration/CacheRegistration.md) for more information on how to do so.

### Cache duration from configuration file

In an effort to make our interfaces more clean while still using custom cache duration, you can leave your cache attribute definition without specifying the cache duration, but that same cache duration comes from a configuration section of _appsettings.json_.

When configuring your [cache usage](../CacheRegistration/CacheRegistration.md), you can specify a specific section from your _appsettings.json_ file where you are going to read all your cache configurations from. By default, the section name is **_RedisConfig_**.

Under the section you defined above, create a new section called _CacheSettings_, where you are going to place the your cache duration.

The key you need to use here should the a template key based upon the assembly your interface implementation lives upon. For more information on what to place on the key part, please refer to [this](../CacheKeyGeneration/CacheKeyGeneration.md).

The value for your cache duration should follow the _HH.MM.SS_ pattern, where HH represents Hours, MM represents Minutes and SS represent seconds. So, for example, if you want your cache to have a duration of 1 hour, 15 minutes and 30 seconds, the value should for the duration should be _01:15:30_.

Below you can find an example of how this could exist in your configuration:

```json
  "RedisConfig": {
    "CacheSettings": {
      "Gorold-Payment-Attributes-RedisStuff--AnotherMethod": "01:15:30",
      "Gorold-Payment-Attributes-RedisStuff--SomeMethod--{param1}--{param2}--{name}": "00:20:00"
    }
  }
```

Let's take a closer look at the structure of the second configuration cache key (same conclusions apply for any kind of key to be inserted).

```
//this is the key we are analyzing
Gorold-Payment-Attributes-RedisStuff--SomeMethod--{param1}--{param2}--{name}

Gorold-Payment-Attributes-RedisStuff -> Namespace where the interface, where the attribute was placed, implementation is located. Notice that we replaced all the '.' for '-'.

--SomeMethod--{param1}--{param2}--{name} -> The remaining part of the key is constitued of the method where the attribute was placed, plus the methods it might contain. If no parameters exist, then we don't need to insert them on the cache key definition. If you notice closely, when you are adding cache duration for methods that take parameters, you must add such parameters inside the curly braces. You should add only the parameter name, not its value. Also notice that, unline above, we use double '-' to separate things.
```