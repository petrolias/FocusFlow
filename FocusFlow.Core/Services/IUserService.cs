using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Models;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FocusFlow.Core.Services
{
    public interface IUserService
    {
        Task<Result<bool>> AssignRoleToUserAsync(AppUser user, string roleName);

        Task<Result<IdentityRole>> CreateRoleAsync(string roleName);

        Task<Result<AppUser>> CreateUserAsync(string email, string password);

        Task<Result<IEnumerable<AppUserDto>>> GetAllAsync();

        Result<AppUserDto> GetById(string id);

        Task<Result<bool>> MapUserFieldsAsync<TModel>(IEnumerable<TModel> models) where TModel : IEntryRecordBaseUser;

        Task<Result<bool>> MapUserFieldsAsync<TModel>(TModel model) where TModel : IEntryRecordBaseUser;
    }
}