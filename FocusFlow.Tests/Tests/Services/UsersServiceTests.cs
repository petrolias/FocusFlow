using FocusFlow.Core.Models;
using FocusFlow.Core.Services;
using FocusFlow.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    public class UsersServiceTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private string Email() => $"{Guid.NewGuid().ToString()}@example.com";
        private const string Password = "Password123!";
        public UsersServiceTests(TestFixture fixture)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _userService = _scope.ServiceProvider.GetRequiredService<IUserService>();
            _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            _roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public void Dispose() => _scope.Dispose(); // Clean up the scope after each test

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated()
        {
            var email = Email();
            var result = await _userService.CreateUserAsync(email, Password);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(email, result.Value.Email);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Not_Allow_Invalid_User()
        {
            var email = Email();

            var result = await _userService.CreateUserAsync(string.Empty, Password);
            Assert.True(!result.IsSuccess);

            result = await _userService.CreateUserAsync(email, string.Empty);
            Assert.True(!result.IsSuccess);

            result = await _userService.CreateUserAsync(string.Empty, string.Empty);
            Assert.True(!result.IsSuccess);
        }

        [Fact]
        public async Task CreateUserAsync_Should_Not_Duplicate_User()
        {
            var email = Email();
            var result = await _userService.CreateUserAsync(email, Password);
            Assert.True(result.IsSuccess);

            result = await _userService.CreateUserAsync(email, Password);
            Assert.True(!result.IsSuccess);
        }


        [Fact]
        public async Task CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
        {
            var roleName = "Admin";
            var result = await _userService.CreateRoleAsync(roleName);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(roleName, result.Value.Name);
        }

        [Fact]
        public async Task AssignRoleToUserAsync_ShouldReturnSuccess_WhenRoleIsAssigned()
        {
            var email = Email();
            var user = new AppUser { UserName = email, Email = email };
            var roleName = "Admin";

            await _userManager.CreateAsync(user, Password);
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            var result = await _userService.AssignRoleToUserAsync(user, roleName);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}