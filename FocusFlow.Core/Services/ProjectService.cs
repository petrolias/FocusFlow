using AutoMapper;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Common;
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
        public async Task<Result<IEnumerable<ProjectDto>>> GetAllAsync()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                if (!projects.Any())
                    return Result<IEnumerable<ProjectDto>>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = _mapper.Map<IEnumerable<ProjectDto>>(projects);
                return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<IEnumerable<ProjectDto>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto?>> GetByIdAsync(Guid id)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(id);
                if (project is null)
                    return Result<ProjectDto?>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = _mapper.Map<ProjectDto>(project);
                return Result<ProjectDto?>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<ProjectDto?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<ProjectDto>> AddAsync(ProjectCreateDto project)
        {
            try
            {
                var model = _mapper.Map<Project>(project);
                var addResult = await _projectRepository.AddAsync(model, true);
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

        public async Task<Result<ProjectDto>> UpdateProjectAsync(ProjecUpdateDto project)
        {
            try
            {
                var model = _mapper.Map<Project>(project);              
                var updateResult = await _projectRepository.UpdateAsync(model, true);
                if (!updateResult.IsSuccess)
                    return Result<ProjectDto>.From(updateResult);

                var result = _mapper.Map<ProjectDto>(project);
                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<ProjectDto>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteProjectAsync(Guid id)
        {
            try
            {
                await _projectRepository.DeleteAsync(id, true);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<bool>(LogLevel.Error, exception: ex);

            }
        }      
    }
}