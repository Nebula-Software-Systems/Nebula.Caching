using Nebula.Caching.Common.Extensions;
using Nebula.Caching.Redis.Extensions;
using RedisSample.Services;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IService, MyService>();

builder.Host.UseNebulaCache();

/*
 * Configure Redis. We require the existing of an IConnectionMultiplexer.
 */
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddRedisChache();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

string[] summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/service/sync", (IService service) => $"Response from the sync endpoint: {service.HelloWorld("Jane")}")
    .WithName("GetWeatherForecastSync");

app.MapGet("/service/async", async (IService service) => $"Response from the async endpoint: {await service.HelloWorldAsync()}")
    .WithName("GetWeatherForecastAsync");

app.MapGet("/service/sum", (IService service) => $"Response from the sum endpoint: {service.SumConfigCache(2,3)}")
    .WithName("GetWeatherForecastSum");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
