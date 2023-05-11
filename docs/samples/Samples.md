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
