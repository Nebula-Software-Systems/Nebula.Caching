using Nebula.Caching.Common.Extensions;
using Nebula.Caching.MemCached.Extensions;
using Nebula.Caching.MemCached.Settings;
using Nebula.Caching.MemcachedSample.Interfaces;
using Nebula.Caching.MemcachedSample.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Start cache configuration
        builder.Host.UseNebulaCaching();
        builder.Services.AddMemCachedChache(new MemCachedConfigurations
        {
            ConfigurationSection = "MemCachedConfig",
            DefaultCacheDurationInSeconds = 120
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