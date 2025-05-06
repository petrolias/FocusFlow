using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Mappings;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.Core.Seeders;
using FocusFlow.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core
{
    public static class Dependecies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection self,
            Action<DbContextOptionsBuilder> dbContextOptions, LogLevel logLevel = LogLevel.Error)
        {
            self.AddDbContext<Context>(dbContextOptions)
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<ITaskItemRepository, TaskItemRepository>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<ITaskItemService, TaskItemService>()
                .AddScoped<IUserService, UserService>()
                .AddAutoMapper(
                    typeof(ProjectDtosMappingProfile),
                    typeof(TaskItemDtosMappingProfile),
                    typeof(AuthDtosMappingProfile))
                .AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<Context>();

            self.AddLogging(builder =>
            {                
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(logLevel);
            });

            return self;
        }        

        public static void ContextMigrate(this IServiceProvider self)
        {
            using var scope = self.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            var provider = context.Database.ProviderName;
            if (provider != "Microsoft.EntityFrameworkCore.InMemory")
                context.Database.Migrate();
        }

        public static async Task SeedDataAsync(this IServiceProvider self)
        {
            using var scope = self.CreateScope();
            await IdentitySeeder.SeedAsync(scope.ServiceProvider);
        }
    }
}