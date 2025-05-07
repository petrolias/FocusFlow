using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class TaskItemService(
        ILogger<ProjectService> logger,
        IMapper mapper,
        IUserService userService,
        ITaskItemRepository taskItemRepository
        ) : ITaskItemService
    {
        public async Task<Result<IEnumerable<TaskItemDto>>> GetAllAsync(bool includeProject = false)
        {
            try
            {
                var getAllResult = await taskItemRepository.GetAllAsync(includeProject);
                if (!getAllResult.IsSuccess)
                    return Result<IEnumerable<TaskItemDto>>.From(getAllResult);

                var result = mapper.Map<IEnumerable<TaskItemDto>>(getAllResult.Value);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<IEnumerable<TaskItemDto>>.From(mapUserResult);

                return Result<IEnumerable<TaskItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<TaskItemDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<TaskItemDto?>> GetByIdAsync(Guid id, bool includeProject = false)
        {
            try
            {
                var getByIdResult = await taskItemRepository.GetByIdAsync(id, includeProject);
                if (!getByIdResult.IsSuccess)
                    return Result<TaskItemDto?>.From(getByIdResult);

                if (getByIdResult.Value is null)
                    return Result<TaskItemDto?>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = mapper.Map<TaskItemDto>(getByIdResult.Value);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<TaskItemDto>.From(mapUserResult);

                return Result<TaskItemDto?>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<TaskItemDto?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<TaskItemDto>>> GetFilteredAsync(TaskItemFilter filter, bool includeProject = false)
        {
            try
            {
                var getFilteredResult = await taskItemRepository.GetFilteredAsync(filter, includeProject);
                if (!getFilteredResult.IsSuccess)
                    return Result<IEnumerable<TaskItemDto>>.From(getFilteredResult);

                var result = mapper.Map<IEnumerable<TaskItemDto>>(getFilteredResult.Value);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<IEnumerable<TaskItemDto>>.From(mapUserResult);

                return Result<IEnumerable<TaskItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<TaskItemDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<TaskItemDto>> AddAsync(TaskItemDtoBase project, string userId)
        {
            try
            {
                var model = mapper.Map<TaskItem>(project);
                model.CreatedAt = DateTimeOffset.UtcNow;
                model.CreatedBy = userId;

                var addResult = await taskItemRepository.AddAsync(model);
                if (!addResult.IsSuccess)
                    return Result<TaskItemDto>.From(addResult);

                var result = mapper.Map<TaskItemDto>(model);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<TaskItemDto>.From(mapUserResult);

                return Result<TaskItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<TaskItemDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<TaskItemDto>> UpdateAsync(Guid id, TaskItemDtoBase taskItem, string userId)
        {
            try
            {
                var existingModelResult = await taskItemRepository.GetByIdAsync(id);
                if (!existingModelResult.IsSuccess)
                    return Result<TaskItemDto>.From(existingModelResult);
                
                if (existingModelResult.Value == null)
                    return Result<TaskItemDto>.Failure(statusCode: StatusCodes.Status404NotFound);

                var model = existingModelResult.Value;
                model.UpdatedAt = DateTimeOffset.UtcNow;
                model.UpdatedBy = userId;
                model.Title = taskItem.Title;
                model.Description = taskItem.Description;
                model.DueDate = taskItem.DueDate;
                model.Status = taskItem.Status;
                model.Priority = taskItem.Priority;
                model.AssignedUserId = taskItem.AssignedUserId;

                var updateResult = await taskItemRepository.UpdateAsync(model);
                if (!updateResult.IsSuccess)
                    return Result<TaskItemDto>.From(updateResult);

                var result = mapper.Map<TaskItemDto>(model);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<TaskItemDto>.From(mapUserResult);

                return Result<TaskItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<TaskItemDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteTaskItemAsync(Guid id, string userId)
        {
            try
            {
                var deleteResult = await taskItemRepository.DeleteAsync(id);
                if (!deleteResult.IsSuccess)
                    return Result<bool>.From(deleteResult);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }
    }
}