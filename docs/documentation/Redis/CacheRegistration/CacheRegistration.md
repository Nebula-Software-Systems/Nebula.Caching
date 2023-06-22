# Registring the cache usage

As mentioned in the [README](../../../../README.md), the first step to start using out our cache is to use the following methods inside the _Program.cs_ class:

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...

      builder.Host.UseNebulaCaching();
      builder.Services.AddRedisChache(new Configurations
      {
        //some amazing configuration options
      });
    }
}

```

As mentioned above, in the _AddRedisChache_ method, there are some configuration options we can use when registering our cache.

The following options are, as of today, supported:

## ConfigurationSection

String that represents the section in your _appsettings.json_ where the cache configuration will be placed. By default the value here is **_"RedisConfig"_**.Example of configurations placed in such section may include the cache duration for your keys (if you end up choosing this path). Please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on cache duration options.

## DefaultCacheDurationInSeconds

Sometimes we don't want to have the hassle to define cache values for each method, and instead just use a pre-defined cache duration value.

If you think the current default value for the cache duration doesn't suit you well (600 seconds), then you have the opportunity to override it by making use of the _DefaultCacheDurationInSeconds_ in the _Configurations_ object.

Insert the int value that represents the new default cache duration, in seconds.

Again, please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on other ways to set the cache duration.

## ConfigurationFlavour

Our cache management process depends on the configuration flavor, which in its essence means who manages the configuration and dependency injection that regards setting up the cache provider (which, as of today, is only Redis).

If you want us to register and manage the configuration of your cache provider, you can choose from one of our configuration flavors. If you want to have "control" of how your cache provider is configured, then you don't need to specify anything under the _ConfigurationFlavour_ property.

Our software currently supports two configuration flavors, which can be set, as stated before, in the ConfigurationFlavour property:

- Vanilla
- Configured

### Vanilla

This configuration flavor frees you from having to register Redis, which means that it will be done behind the scenes for you. This is the simplest configuration, which only requires you to have a section in your configuration to indicate where your cache provider is running and, optionally, a _Log_ of type _TextWriter_. You can pass this logger via the _Log_ property (which we will mention down below).

For our cache service registration to properly work, you need to have in your configuration section the connection string where your Redis service is hosted. You need to place it under the configuration key _CacheServiceUrl_, which is where our software will read from to reach your Redis service.

### Configured

If using our Vanilla option is not enough for you and you want to add more advanced configuration to Redis, you can do so by using our Configured flavor.

With this, you can pass more advanced configuration, like the _Configure_ or the _Configuration_ that we will speak below, and which are tightly bound to Redis.

There are a couple of scenarios to have in consideration when using this configuration flavor:

1. Using the **_Configuration_** property as the source of configuration

   - In this case, you'll need to set the property and such property will be used to configure Redis. Optionally, you can also set the _Log_ property. You don't need to have the _CacheServiceUrl_ cache section defined in your _appsettings.json_.

2. Not using the **_Configuration_** property. This means that:
   - You need to have to have the _CacheServiceUrl_ cache section defined in your _appsettings.json_.
   - You need to configure the **_Configure_** property.
   - Optionally, you can also configure the **_Log_** property.

## Configure

This property represents the **_Configure_** property of the Redis library. This property is of type _Action<ConfigurationOptions>_.

According to their documentation, the _Configure_ property is:

> Action to further modify the parsed configuration options.

Also according to their documentation, _ConfigurationOptions_ is:

> The options relevant to a set of redis connections.

## Configuration

This property represents the **_Configuration_** property of the Redis library, which is of type _ConfigurationOptions_.

According to their documentation, the **_Configuration_** property is:

> The configuration options to use for this multiplexer.

## Log

This property represents the **_Log_** property of the Redis library, which is of type _TextWriter_.

According to their documentation, the **_Log_** property is:

> The System.IO.TextWriter to log to.

## Example of a cache registration

Below you can find an example of what the cache registration could look like in your _Program.cs_ class, having all we discussed in consideration:

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

Because we are using the _Vanilla_ configuration flavor, we need a configuration section that would have the URL to our Redis cache service:

```json

"RedisConfig": {
    "CacheServiceUrl" : "localhost",
    "CacheSettings" : {
      // our cache duration configuration
    }
  }

```

## Small note if you decide to manage your Redis registrations

Regardless of the way you manage your Redis registrations, please make sure that you register the type _IConnectionMultiplexer_, because behind the scenes we use this to interact with the Redis cache.
