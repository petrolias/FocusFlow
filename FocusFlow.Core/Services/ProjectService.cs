using AutoMapper;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class ProjectService(
        ILogger<ProjectService> _logger,
        IMapper _mapper,
        IProjectRepository _projectRepository) : IProjectService
    {
        public async Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(bool includeTask = false)
        {
            try
            {
                var getAllResult = await _projectRepository.GetAllAsync(includeTask);
                if (!getAllResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(getAllResult);

                var result = _mapper.Map<IEnumerable<ProjectDto>>(getAllResult.Value);
                return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<IEnumerable<ProjectDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto?>> GetByIdAsync(Guid id, bool includeTask = false)
        {
            try
            {
                var getByIdResult = await _projectRepository.GetByIdAsync(id, includeTask);
                if (!getByIdResult.IsSuccess)
                    return Result<ProjectDto?>.From(getByIdResult);

                if (getByIdResult.Value is null)
                    return Result<ProjectDto?>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = _mapper.Map<ProjectDto>(getByIdResult.Value);
                return Result<ProjectDto?>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<ProjectDto?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<ProjectDto>>> GetFilteredAsync(ProjectFilter filter, bool includeProject = false)
        {
            try
            {
                var getFilteredResult = await _projectRepository.GetFilteredAsync(filter, includeProject);
                if (!getFilteredResult.IsSuccess)
                    return Result<IEnumerable<ProjectDto>>.From(getFilteredResult);

                var result = _mapper.Map<IEnumerable<ProjectDto>>(getFilteredResult.Value);
                return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<IEnumerable<ProjectDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto>> AddAsync(ProjectDtoBase project, string userId)
        {
            try
            {
                var model = _mapper.Map<Project>(project);
                model.CreatedAt = DateTimeOffset.UtcNow;
                model.CreatedBy = userId;

                var addResult = await _projectRepository.AddAsync(model);
                if (!addResult.IsSuccess)
                    return Result<ProjectDto>.From(addResult);

                var result = _mapper.Map<ProjectDto>(model);
                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<ProjectDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto>> UpdateAsync(Guid id, ProjectDtoBase project, string userId)
        {
            try
            {
                var existingModelResult = await _projectRepository.GetByIdAsync(id);
                if (!existingModelResult.IsSuccess)
                    return Result<ProjectDto>.From(existingModelResult);

                var model = existingModelResult.Value;
                model.UpdatedAt = DateTimeOffset.UtcNow;
                model.UpdatedBy = userId;
                model.Name = project.Name;
                model.Description = project.Description;

                var updateResult = await _projectRepository.UpdateAsync(model);
                if (!updateResult.IsSuccess)
                    return Result<ProjectDto>.From(updateResult);

                var result = _mapper.Map<ProjectDto>(model);
                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<ProjectDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteProjectAsync(Guid id, string userId)
        {
            try
            {
                var deleteResult = await _projectRepository.DeleteAsync(id);
                if (!deleteResult.IsSuccess)
                    return Result<bool>.From(deleteResult);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }
    }
}