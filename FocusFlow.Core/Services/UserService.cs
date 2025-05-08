using System.Data;
using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Models;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class UserService(
        ILogger<UserService> logger,
        IMapper mapper,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IUserService
    {
        public async Task<Result<AppUserDto>> CreateUserAsync(string email, string password)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                if (string.IsNullOrEmpty(email))
                    return Result<AppUserDto>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Email cannot be empty.");

                if (string.IsNullOrEmpty(password))
                    return Result<AppUserDto>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Password cannot be empty.");

                if (await userManager.FindByEmailAsync(email) != null)
                    return Result<AppUserDto>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "User already exists.");

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<AppUserDto>.Failure(statusCode: StatusCodes.Status400BadRequest, message: $"Failed to create user: {errors}");
                }

                var dto = mapper.Map<AppUserDto>(user);
                return Result<AppUserDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<AppUserDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<string>> CreateRoleAsync(string roleName)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return Result<string>.Success(role.Name);
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return Result<string>.Failure(statusCode: StatusCodes.Status400BadRequest, message: $"Failed to create role: {errors}");
                    }
                }
                else
                {
                    return Result<string>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Role already exists.");
                }
            }
            catch (Exception ex)
            {
                return logger.FailureLog<string>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> AssignRoleToUserAsync(string userId, string roleName)
        {
            try
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    var user = await userManager.FindByIdAsync(userId.ToString());
                    var result = await userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        return Result<bool>.Success(true);
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return Result<bool>.Failure(statusCode: StatusCodes.Status400BadRequest, message: $"Failed to assign role to user: {errors}");
                    }
                }
                else
                {
                    return Result<bool>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Role does not exist.");
                }
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<AppUserDto>>> GetAllAsync()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();
                var result = mapper.Map<IEnumerable<AppUserDto>>(users);
                return Result<IEnumerable<AppUserDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<AppUserDto>>(LogLevel.Error, exception: ex);
            }
        }

        /// <summary>
        /// Used in automapper resolver
        /// </summary>
        public Result<AppUserDto> GetById(string id)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(x => x.Id.ToString() == id.ToString());
                user ??= new();
                var result = mapper.Map<AppUserDto>(user);
                return Result<AppUserDto>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<AppUserDto>(LogLevel.Error, exception: ex);
            }
        }

        /// <summary>
        /// Maps user fields in the model to the user email
        /// </summary>
        public async Task<Result<bool>> MapUserFieldsAsync<TModel>(TModel model) where TModel : IEntryRecordBaseUser
            => await MapUserFieldsAsync(new List<TModel> { model });

        /// <summary>
        /// Maps user fields in the model to the user email
        /// </summary>
        public async Task<Result<bool>> MapUserFieldsAsync<TModel>(IEnumerable<TModel> models) where TModel : IEntryRecordBaseUser
        {
            try
            {
                var usersResult = await this.GetAllAsync();
                if (!usersResult.IsSuccess)
                    return Result<bool>.From(usersResult);
                foreach (var model in models)
                {
                    if (model == null)
                        continue;

                    if (model.CreatedBy != null)
                        model.CreatedByInfo = usersResult.Value.FirstOrDefault(x => x.Id == model.CreatedBy)?.Email ?? string.Empty;
                    if (model.UpdatedBy != null)
                        model.UpdatedByInfo = usersResult.Value.FirstOrDefault(x => x.Id == model.UpdatedBy)?.Email ?? string.Empty;
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }
    }
}