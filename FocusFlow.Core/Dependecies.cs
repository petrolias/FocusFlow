using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Identity;
using FocusFlow.Core.Mappings;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Core
{
    public static class Dependecies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection self, string connectionString)
        {
            self.AddDbContext<Context>(options => options.UseSqlite(connectionString))
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<ITaskItemRepository, TaskItemRepository>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<ITaskItemService, TaskItemService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddAutoMapper(typeof(ProjectDtosMappingProfile), typeof(TaskItemDtosMappingProfile))
                .AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<Context>();                

            return self;
        }        

        public static void ContextMigrate(this IServiceProvider self)
        {
            using (var scope = self.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                dbContext.Database.Migrate();
            }
        }

        public static async Task SeedDataAsync(this IServiceProvider self)
        {
            using (var scope = self.CreateScope())
            {
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
            }
        }
    }
}