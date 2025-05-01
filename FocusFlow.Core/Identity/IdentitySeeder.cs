using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Core.Identity
{
    internal class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Optional: create a role
            const string roleName = "Admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Create a default user
            var defaultUser = await userManager.FindByEmailAsync("admin@example.com");
            if (defaultUser == null)
            {
                var user = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin123$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    // Optional: log errors
                    throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
