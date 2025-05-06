using System.Data;
using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class IdentityService(
        ILogger<IdentityService> logger,
        IMapper mapper,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IIdentityService
    {
        public async Task<Result<AppUser>> CreateUserAsync(string email, string password)
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
                    return Result<AppUser>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Email cannot be empty.");

                if (string.IsNullOrEmpty(password))
                    return Result<AppUser>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Password cannot be empty.");

                if (await userManager.FindByEmailAsync(email) != null)
                    return Result<AppUser>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "User already exists.");

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    return Result<AppUser>.Success(user);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<AppUser>.Failure(statusCode: StatusCodes.Status400BadRequest, message: $"Failed to create user: {errors}");
                }
            }
            catch (Exception ex)
            {
                return logger.FailureLog<AppUser>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IdentityRole>> CreateRoleAsync(string roleName)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return Result<IdentityRole>.Success(role);
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return Result<IdentityRole>.Failure(statusCode: StatusCodes.Status400BadRequest, message: $"Failed to create role: {errors}");
                    }
                }
                else
                {
                    return Result<IdentityRole>.Failure(statusCode: StatusCodes.Status400BadRequest, message: "Role already exists.");
                }
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IdentityRole>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> AssignRoleToUserAsync(AppUser user, string roleName)
        {
            try
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
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

        public async Task<Result<IEnumerable<AppUserDto>>> GetAllUsersAsync()
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
    }
}