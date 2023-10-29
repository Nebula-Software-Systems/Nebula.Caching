# Registring the cache usage

As mentioned in the [README](../../../../README.md), the first step to start using out our cache is to register the cache usage inside the _Program.cs_ class:

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...

      builder.Host.UseNebulaCaching();
      builder.Services.AddInMemoryChache(new InMemoryConfigurations
      {
        //some amazing optional configuration options
      });
    }
}

```

As mentioned above, in the _AddInMemoryChache_ method, there are some configuration options we can use when registering our cache.

The following options are, as of today, supported:

## ConfigurationSection

String that represents the section in your _appsettings.json_ where the cache configuration will be placed. By default the value here is **_"InMemory"_**. Example of configurations placed in such section may include the cache duration for your keys (if you end up choosing this path). Please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on cache duration options.

## DefaultCacheDurationInSeconds

Sometimes we don't want to have the hassle to define cache values for each method, and instead just use a pre-defined cache duration value.

If you think the current default value for the cache duration doesn't suit you well (600 seconds), then you have the opportunity to override it by making use of the _DefaultCacheDurationInSeconds_ in the _Configurations_ object.

Insert the int value that represents the new default cache duration, in seconds.

Again, please refer to [this](../AttributeUsage/AttributeUsage.md) for more information on other ways to set the cache duration.

## Example of a cache registration

Below you can find an example of what the cache registration could look like in your _Program.cs_ class, having all we discussed in consideration:

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...
        builder.Host.UseNebulaCaching();
        builder.Services.AddInMemoryChache(new InMemoryConfigurations
        {
            ConfigurationSection = "InMemoryConfig",
            DefaultCacheDurationInSeconds = 120
        });
    }
}

```
