using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class ProjectService(
        ILogger<ProjectService> logger,
        IMapper mapper,
        IUserService userService,
        IProjectRepository projectRepository) : IProjectService
    {
        public async Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(bool includeTask = false)
        {
            try
            {
                var getAllResult = await projectRepository.GetAllAsync(includeTask);
                if (!getAllResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(getAllResult);

                var result = mapper.Map<IEnumerable<ProjectDto>>(getAllResult.Value);

                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(mapUserResult);

                return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<ProjectDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto?>> GetByIdAsync(Guid id, bool includeTask = false)
        {
            try
            {
                var getByIdResult = await projectRepository.GetByIdAsync(id, includeTask);
                if (!getByIdResult.IsSuccess)
                    return Result<ProjectDto?>.From(getByIdResult);

                if (getByIdResult.Value is null)
                    return Result<ProjectDto?>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = mapper.Map<ProjectDto>(getByIdResult.Value);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<ProjectDto?>.From(mapUserResult);

                return Result<ProjectDto?>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<ProjectDto?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<ProjectDto>>> GetFilteredAsync(ProjectFilter filter, bool includeTask = false)
        {
            try
            {
                var getFilteredResult = await projectRepository.GetFilteredAsync(filter, includeTask);
                if (!getFilteredResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(getFilteredResult);

                var result = mapper.Map<IEnumerable<ProjectDto>>(getFilteredResult.Value);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(mapUserResult);

                return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<ProjectDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto>> AddAsync(ProjectDtoBase project, string userId)
        {
            try
            {
                var model = mapper.Map<Project>(project);
                model.CreatedAt = DateTimeOffset.UtcNow;
                model.CreatedBy = userId;

                var addResult = await projectRepository.AddAsync(model);
                if (!addResult.IsSuccess)
                    return Result<ProjectDto>.From(addResult);

                var result = mapper.Map<ProjectDto>(model);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<ProjectDto>.From(mapUserResult);

                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<ProjectDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto>> UpdateAsync(Guid id, ProjectDtoBase project, string userId)
        {
            try
            {
                var existingModelResult = await projectRepository.GetByIdAsync(id);
                if (!existingModelResult.IsSuccess)
                    return Result<ProjectDto>.From(existingModelResult);

                if (existingModelResult.Value == null)
                    return Result<ProjectDto>.Failure(statusCode: StatusCodes.Status404NotFound);

                var model = existingModelResult.Value;
                model.UpdatedAt = DateTimeOffset.UtcNow;
                model.UpdatedBy = userId;
                model.Name = project.Name;
                model.Description = project.Description;

                var updateResult = await projectRepository.UpdateAsync(model);
                if (!updateResult.IsSuccess)
                    return Result<ProjectDto>.From(updateResult);

                var result = mapper.Map<ProjectDto>(model);
                var mapUserResult = await userService.MapUserFieldsAsync(result);
                if (!mapUserResult.IsSuccess)
                    return Result<ProjectDto>.From(mapUserResult);

                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<ProjectDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteProjectAsync(Guid id, string userId)
        {
            try
            {
                var deleteResult = await projectRepository.DeleteAsync(id);
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