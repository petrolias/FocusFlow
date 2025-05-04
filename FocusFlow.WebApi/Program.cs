using Microsoft.EntityFrameworkCore;
using FocusFlow.Core;
using FocusFlow.WebApi.HealthChecks;
using static FocusFlow.WebApi.HealthChecks.HealthCheckExtensions;

namespace FocusFlow.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.AddHealthChecks();

            app.Services.ContextMigrate();

            await app.Services.SeedDataAsync();

            app.Run();
        }

        public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder)
        {
            //Specify the application debug levels if needed
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .AddDebug();

            // Add services to the container.
            builder.Services.AddControllers();            
            builder.Services.AddHealthChecks()
                    .AddCheck<DatabaseHealthCheck>("Database", tags: new[] { HealthCheckType.Readiness.ToString() });
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddDependencies(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

            return builder;
        }
    }
}