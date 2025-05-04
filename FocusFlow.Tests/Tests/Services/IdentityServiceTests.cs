using FocusFlow.Core.Models;
using FocusFlow.Core.Services;
using FocusFlow.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    public class IdentityServiceTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IIdentityService _identityService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityServiceTests(TestFixture fixture)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _identityService = _scope.ServiceProvider.GetRequiredService<IIdentityService>();
            _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            _roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public void Dispose() => _scope.Dispose(); // Clean up the scope after each test

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated()
        {
            var email = "test@example.com";
            var password = "Password123!";

            var result = await _identityService.CreateUserAsync(email, password);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(email, result.Value.Email);
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
        {
            var roleName = "Admin";

            var result = await _identityService.CreateRoleAsync(roleName);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(roleName, result.Value.Name);
        }

        [Fact]
        public async Task AssignRoleToUserAsync_ShouldReturnSuccess_WhenRoleIsAssigned()
        {
            var user = new AppUser { UserName = "testuser" };
            var roleName = "Admin";

            await _userManager.CreateAsync(user, "Password123!");
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            var result = await _identityService.AssignRoleToUserAsync(user, roleName);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}