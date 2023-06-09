<h1 align="center">
  Nebula.Caching
</h1>
<p align="center">
  Make use of caching without any hassle.
</p>

## About

Nebula.Caching is an Open-Source caching library that allows you to use caching in your projects with minimal configuration.

As of today, we support caching using Redis and InMemory, but in the future we hope to support your favourite caching provider!

## Nuget Package

| Name                    | Released Package                                                                                                                                                        |
| ----------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Nebula-Caching-Redis    | [![BotBuilder Badge](https://buildstats.info/nuget/Nebula-Caching-Redis?includePreReleases=true&dWidth=70)](https://www.nuget.org/packages/Nebula-Caching-Redis/)       |
| Nebula-Caching-InMemory | [![BotBuilder Badge](https://buildstats.info/nuget/Nebula-Caching-InMemory?includePreReleases=true&dWidth=70)](https://www.nuget.org/packages/Nebula-Caching-InMemory/) |

## Usage

### Step 1 : Install one of the available packages

Install the package via Package Manager:

```
Install-Package Nebula-Caching-Redis
Install-Package Nebula-Caching-InMemory
```

or

Via .NET CLI:

```
dotnet add package Nebula-Caching-Redis
dotnet add package Nebula-Caching-InMemory
```

### Step 2 : Register cache usage in the Program class (will change depending on which caching provider you are using, please refer to the package documentation)

```csharp

public class Program
{
    public static void Main(string[] args)
    {
      // ...

      builder.Host.UseNebulaCaching();
      builder.Services.AddRedisChache(new Configurations
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

## CI Status

| Branch                                                                          | Build Status                                                                                                                                                                                             | Test Coverage                                                                                                                                                                                                       |
| ------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [**Main**](https://github.com/Nebula-Software-Systems/Nebula.Caching/tree/main) | [![DotNet Main](https://github.com/Nebula-Software-Systems/Nebula.Caching/actions/workflows/cicd.yaml/badge.svg)](https://github.com/Nebula-Software-Systems/Nebula.Caching/actions/workflows/cicd.yaml) | [![Coverage Status](https://coveralls.io/repos/github/Nebula-Software-Systems/Nebula.Caching/badge.svg?branch=main&service=github)](https://coveralls.io/github/Nebula-Software-Systems/Nebula.Caching?branch=main) |

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
