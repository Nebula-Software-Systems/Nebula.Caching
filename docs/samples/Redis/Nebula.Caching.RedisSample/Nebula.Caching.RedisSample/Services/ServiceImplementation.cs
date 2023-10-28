using Nebula.Caching.RedisSample.Interfaces;

namespace Nebula.Caching.RedisSample.Services;

public class ServiceImplementation : IService
{
    public string OneMethod(string name, int year)
    {
        return $"{name} was born in {year}.";
    }

    public int MagicMethod()
    {
        return 2023;
    }

    public ComplexObject AnotherMethod(string someParameter)
    {
        return new ComplexObject
        {
            Age = 2023,
            Name = someParameter
        };
    }

    public bool SomeMethod()
    {
        return false;
    }
}