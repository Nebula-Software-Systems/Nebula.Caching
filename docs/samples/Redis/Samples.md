# Code samples

### Using the simple configuration scenario (_Vanilla_), with a default cache duration of 120 seconds, and the cache configuration in appsettings.json will occur in the section _RedisConfig_

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...
        builder.Host.UseNebulaCaching();
        builder.Services.AddRedisChache(new Configurations
        {
            ConfigurationSection = "RedisConfig",
            ConfigurationFlavour = RedisConfigurationFlavour.Vanilla,
            DefaultCacheDurationInSeconds = 120
        });
    }
}

```

The _appsettings.json_ file should have the configured section:

```json
 "RedisConfig": {
    "CacheServiceUrl": "localhost",
    "CacheSettings": {
        //whatver cache keys are configured with respective cache duration
    }
  }
```

### Redis configuration is managed by the library user, with a default cache duration of 50 seconds, and the cache configuration in appsettings.json will occur in the section _MyCacheConfigSection_

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...

        //since managing Redis is not done by the library, we must have *IConnectionMultiplexer* injected for the library to properly work
        builder.Services.AddSingleton<IConnectionMultiplexer>(ctx =>
        {
            return ConnectionMultiplexer.Connect("<redis-service-connection-string>");
        });
        builder.Host.UseNebulaCaching();
        builder.Services.AddRedisChache(new Configurations
        {
            ConfigurationSection = "MyCacheConfigSection",
            DefaultCacheDurationInSeconds = 50
        });
    }
}

```

The _appsettings.json_ file should have the configured section:

```json
 "MyCacheConfigSection": {
    "CacheSettings": {
        //whatver cache keys are configured with respective cache duration
    }
  }
```

### After configuring your application with any of the configurations above, you start configuring your attributes to use cache

Please check [this documentation file](../documentation/AttributeUsage/AttributeUsage.md) to have a lot of samples and use cases about some variety of attribute configurations.

### Project that uses Nebula.Caching

Please check [this project](https://github.com/Gorold-Streaming-Services/Gorold.Payment/blob/main/Gorold.Payment/Program.cs) for a real use case of our caching solution.
