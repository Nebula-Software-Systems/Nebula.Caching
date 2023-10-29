using Nebula.Caching.MemcachedSample.Interfaces;

namespace Nebula.Caching.MemcachedSample.Services
{
    public class ServiceImplementation : IService
    {
        public string OneMethod(string name, int year)
        {
            Console.WriteLine($"Method {nameof(OneMethod)} was executed.");
            return $"{name} was born in {year}.";
        }

        public int MagicMethod()
        {
            Console.WriteLine($"Method {nameof(MagicMethod)} was executed.");
            return 2023;
        }

        public ComplexObject AnotherMethod(string someParameter)
        {
            Console.WriteLine($"Method {nameof(AnotherMethod)} was executed.");
            return new ComplexObject
            {
                Age = 2023,
                Name = someParameter
            };
        }

        public bool SomeMethod()
        {   
            Console.WriteLine($"Method {nameof(SomeMethod)} was executed.");
            return false;
        }
    }
}