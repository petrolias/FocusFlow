using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FocusFlow.Core.Services
{
    public interface IIdentityService
    {
        Task<Result<bool>> AssignRoleToUserAsync(AppUser user, string roleName);

        Task<Result<IdentityRole>> CreateRoleAsync(string roleName);

        Task<Result<AppUser>> CreateUserAsync(string email, string password);
        Task<Result<IEnumerable<AppUserDto>>> GetAllUsersAsync();
    }
}