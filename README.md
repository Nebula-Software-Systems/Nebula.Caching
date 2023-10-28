<h1 align="center">
  Nebula.Caching
</h1>
<p align="center">
  Make use of caching without any hassle.
</p>

## About

Nebula.Caching is an Open-Source caching library that allows you to use caching in your projects with minimal configuration.

As of today, we support caching using Redis, InMemory and Memcached, but in the future we hope to support your favourite caching provider!

## Nuget Package

| Name                     | Released Package                                                                                                                                                          |
| ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NebulaCaching.Redis     | [![BotBuilder Badge](https://buildstats.info/nuget/NebulaCaching.Redis?includePreReleases=true&dWidth=70)](https://www.nuget.org/packages/Caching.Nebula.Redis/)         |
| NebulaCaching.InMemory  | [![BotBuilder Badge](https://buildstats.info/nuget/NebulaCaching.InMemory?includePreReleases=true&dWidth=70)](https://www.nuget.org/packages/Caching.Nebula.InMemory)   |
| NebulaCaching.Memcached | [![BotBuilder Badge](https://buildstats.info/nuget/NebulaCaching.Memcached?includePreReleases=true&dWidth=70)](https://www.nuget.org/packages/Caching.Nebula.Memcached/) |

## Usage

### Step 1 : Install one of the available packages

Install the package via Package Manager:

```
Install-Package NebulaCaching.Redis
Install-Package NebulaCaching.InMemory
Install-Package NebulaCaching.Memcached
```

or

Via .NET CLI:

```
dotnet add package NebulaCaching.Redis
dotnet add package NebulaCaching.InMemory
dotnet add package NebulaCaching.Memcached
```

### Step 2 : Register cache usage in the Program class (will change depending on which caching provider you are using, please refer to the package documentation)

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...

      builder.Host.UseNebulaCaching();
      builder.Services.AddRedisChache(new RedisConfigurations
      {
        //some amazing configuration options which can be see in the samples or documentation section
      });
    }
}

```

### Step 3 : Use the caching attribute in your interface definitions (will change depending on which caching provider you are using, please refer to the package documentation)

```csharp

    public interface IRedisStuff
    {
        [RedisCache(CacheDurationInSeconds = 120)]
        List<SomeObject> SomeMethod(int param1, int param2);
    }

```

## Documentation

Our documentation can be found [here](docs/documentation/).

## Samples

Some useful code snippets can be found [here](docs/samples/).

## Roadmap

To check what we want to achieve in the future, please refer to our [ROADMAP](docs/roadmap/Roadmap.md).

## Contributing

This project welcomes and appreciates any contributions made.

There are several ways you can contribute, namely:

- Report any bug found.
- Suggest some features or improvements.
- Creating pull requests.

## License

Nebula.Caching is a free and open-source software licensed under the MIT License.

See [LICENSE](LICENSE) for more details.
