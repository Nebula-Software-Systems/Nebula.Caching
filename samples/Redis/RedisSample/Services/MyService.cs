namespace RedisSample.Services;

public class MyService : IService
{
    public Task<string> HelloWorldAsync() => Task.FromResult("Hello World Async!");

    public string HelloWorld(string name) => $"Hello World Sync! {name}";
    public int SumConfigCache(int num1, int num2) => num1 + num2;
}
