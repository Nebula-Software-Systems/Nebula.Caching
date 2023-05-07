# Registring the cache usage

As mentioned in the [README](../../../README.md), the first step to start using out our cache is to use the following methods inside the *Program.cs* class:

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

As mentioned above, in the *AddRedisChache* method, there are some configuration options we can use when registering our cache.

The following options are, as of today, supported:

## ConfigurationSection
String that represents the section in your *appsettings.json* where the cache configuration will be placed. By default the value here is ***"RedisConfig"***.Example of configurations placed in such section may include the cache duration for your keys (if you end up choosing this path). Please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on cache duration options.

## DefaultCacheDurationInSeconds
Sometimes we don't want to have the hassle to define cache values for each method, and instead just use a pre-defined cache duration value.

If you think the current default value for the cache duration doesn't suit you well (30 seconds), then you have the opportunity to override it by making use of the *DefaultCacheDurationInSeconds* in the *Configurations* object.

Insert the int value that represents the new default cache duration, in seconds.

Again, please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on other ways to set the cache duration.

## ConfigurationFlavour

## Configure

## Configuration

## Log