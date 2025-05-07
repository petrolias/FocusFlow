using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Extensions;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Models;
using FocusFlow.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Core.Seeders
{
    /// <summary>
    /// Seeder helper class to populate the database with initial data.
    /// </summary>
    internal class ContextSeeder
    {

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var isFirstTimeSeed = await SeedUsersAsync(serviceProvider);
            if (!isFirstTimeSeed)
                return;

            await SeedProjectsAsync(serviceProvider);
            await SeedTasksAsync(serviceProvider);
        }

        private static async Task<bool> SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            const string roleName = "Admin";
            const string email = "admin@example.com";
            const string password = "Admin123$";

            // Ensure role exists
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Check if the user already exists
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create seed user: {errors}");
                }
                return true;
            }

            return false;
        }

        private static async Task SeedProjectsAsync(IServiceProvider serviceProvider)
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var projectService = serviceProvider.GetRequiredService<IProjectService>();

            var usersResult = await userService.GetAllAsync();
            var user = usersResult.Value.First();

            for (int i = 0; i < 5; i++)
            {
                var projectDto = new ProjectDtoBase { Name = $"New Project {i}", Description = $"Description {i}" };
                var result = await projectService.AddAsync(projectDto, user.Id);
            }
        }

        private static async Task SeedTasksAsync(IServiceProvider serviceProvider)
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var projectService = serviceProvider.GetRequiredService<IProjectService>();
            var taskItemService = serviceProvider.GetRequiredService<ITaskItemService>();

            var usersResult = await userService.GetAllAsync();
            var user = usersResult.Value.First();

            var projectsResult = await projectService.GetAllAsync();
            var projectIndex = 0;
            foreach (var item in projectsResult.Value)
            {
                for (int i = 0; i < 5; i++)
                {
                    var taskItem = new TaskItemDtoBase
                    {
                        Title = $"Task {i}_{projectIndex}",
                        Description = $"Description {i}_{projectIndex}",
                        Status = EnumExtensions.GetRandomValue<TaskItemStatusEnum>(),
                        Priority = EnumExtensions.GetRandomValue<TaskItemPriorityEnum>(),
                        DueDate = DateTime.UtcNow.AddDays(7),
                        ProjectId = item.Id,
                        AssignedUserId = user.Id
                    };

                    var result = await taskItemService.AddAsync(taskItem, user.Id);
                }
                projectIndex++;
            }
        }
    }
}
