using Nebula.Caching.InMemorySample.Interfaces;

namespace Nebula.Caching.InMemorySample.Services;

public class ServiceImplementation : IService
{
    public string OneMethod(string name, int year)
    {
        Console.WriteLine($"Method {nameof(OneMethod)} executed.");
        return $"{name} was born in {year}.";
    }

    public int MagicMethod()
    {
        Console.WriteLine($"Method {nameof(MagicMethod)} executed.");
        return 2023;
    }

    public ComplexObject AnotherMethod(string someParameter)
    {
        Console.WriteLine($"Method {nameof(AnotherMethod)} executed.");
        return new ComplexObject
        {
            Age = 2023,
            Name = someParameter
        };
    }

    public bool SomeMethod()
    {
        Console.WriteLine($"Method {nameof(SomeMethod)} executed.");
        return false;
    }
}