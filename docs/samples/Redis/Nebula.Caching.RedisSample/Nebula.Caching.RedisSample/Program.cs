using Nebula.Caching.Common.Extensions;
using Nebula.Caching.Redis.Extensions;
using Nebula.Caching.Redis.Settings;
using Nebula.Caching.RedisSample.Interfaces;
using Nebula.Caching.RedisSample.Services;
using Redis.Settings;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Start cache configuration
        builder.Host.UseNebulaCaching();
        builder.Services.AddRedisChache(new RedisConfigurations
        {
            DefaultCacheDurationInSeconds = 3600,
            ConfigurationFlavour = RedisConfigurationFlavour.Vanilla,
            ConfigurationSection = "RedisConfig"
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