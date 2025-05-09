﻿using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<bool>> AssignRoleToUserAsync(string userId, string roleName);

        Task<Result<string>> CreateRoleAsync(string roleName);

        Task<Result<AppUserDto>> CreateUserAsync(string email, string password);

        Task<Result<IEnumerable<AppUserDto>>> GetAllAsync();

        Result<AppUserDto> GetById(string id);

        Task<Result<bool>> MapUserFieldsAsync<TModel>(IEnumerable<TModel> models) where TModel : IEntryRecordBaseUser;

        Task<Result<bool>> MapUserFieldsAsync<TModel>(TModel model) where TModel : IEntryRecordBaseUser;
    }
}