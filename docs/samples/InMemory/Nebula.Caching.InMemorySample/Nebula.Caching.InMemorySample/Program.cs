using Nebula.Caching.Common.Extensions;
using Nebula.Caching.InMemory.Extensions;
using Nebula.Caching.InMemory.Settings;
using Nebula.Caching.InMemorySample.Interfaces;
using Nebula.Caching.InMemorySample.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Start cache configuration
        builder.Host.UseNebulaCaching();
        builder.Services.AddInMemoryChache(new InMemoryConfigurations
        {
            ConfigurationSection = "InMemoryConfig",
            DefaultCacheDurationInSeconds = 3600
        });
        // End cache configuration
        
        builder.Services.AddScoped<IService, ServiceImplementation>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}