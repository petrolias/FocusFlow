using Microsoft.EntityFrameworkCore;
using FocusFlow.Core;
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

            if (!app.Environment.IsEnvironment("Testing"))
                app.Services.ContextMigrate();
           
            await app.Services.SeedDataAsync();

            app.Run();         
        }
        public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder) {
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .AddDebug();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddDependencies(builder.Configuration.GetConnectionString("SqliteConnection") ?? string.Empty);  

            return builder;
        }
        //public static async Task Main(string[] args)
        //{
        //    var builder = CreateHostBuilder(args).Build();

        //    builder.Services.ContextMigrate();
        //    await builder.Services.SeedDataAsync();

        //    builder.Run();
        //}

        //public static WebApplicationBuilder CreateHostBuilder(string[] args)
        //{
        //    var builder = WebApplication.CreateBuilder(args);

        //    builder.Logging
        //        .ClearProviders()
        //        .AddConsole()
        //        .AddDebug();

        //    // Add services to the container.
        //    builder.Services.AddControllers();
        //    builder.Services
        //        .AddEndpointsApiExplorer()
        //        .AddSwaggerGen()
        //        .AddDependencies(builder.Configuration.GetConnectionString("SqliteConnection")??string.Empty)
        //        .AddScoped<IProjectService, ProjectService>()
        //        .AddScoped<ITaskItemService, TaskItemService>()
        //        .AddAutoMapper(typeof(ProjectDtosMappingProfile),typeof(TaskItemDtosMappingProfile));

        //    var app = builder.Build();

        //    if (app.Environment.IsDevelopment())
        //    {
        //        app.UseSwagger();
        //        app.UseSwaggerUI();
        //    }

        //    app.UseHttpsRedirection();
        //    app.UseAuthorization();
        //    app.MapControllers();

        //    return builder;
        //}
    }
}