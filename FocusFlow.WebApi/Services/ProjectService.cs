using System.Runtime.CompilerServices;
using AutoMapper;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.WebApi.DTOs;

namespace FocusFlow.WebApi.Services
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
                return LogError<IEnumerable<ProjectDto>>(ex);
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
                return LogError<ProjectDto?>(ex);
            }
        }

        public async Task<Result<ProjectDto>> AddAsync(ProjectCreateDto project)
        {
            try
            {
                var model = _mapper.Map<Project>(project);
                if (await _projectRepository.GetByIdAsync(model.Id) != null)
                {
                    var msg = this.Caller($"Trying to insert duplicate item with {nameof(model.Id)} : {model.Id}");
                    throw new Exception(msg);
                }

                if (await _projectRepository.GetByProjectNameAsync(model.Name) != null)
                {
                    var msg = this.Caller($"Project with {nameof(model.Name)} : {model.Name} already exists");
                    _logger.LogError(msg);
                    return Result<ProjectDto>.Failure(msg, statusCode: StatusCodes.Status400BadRequest);
                }

                await _projectRepository.AddAsync(model, true);

                var result = _mapper.Map<ProjectDto>(model);
                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<ProjectDto>(ex);
            }
        }

        public async Task<Result<ProjectDto>> UpdateProjectAsync(ProjecUpdateDto project)
        {
            try
            {
                var model = _mapper.Map<Project>(project);

                var sameNameProject = await _projectRepository.GetByProjectNameAsync(model.Name);
                if (sameNameProject != null && sameNameProject.Id != model.Id)
                {
                    var msg = this.Caller($"Project with {nameof(model.Name)} : {model.Name} already exists");
                    _logger.LogError(msg);
                    return Result<ProjectDto>.Failure(msg, statusCode: StatusCodes.Status400BadRequest);

                }
                await _projectRepository.UpdateAsync(model, true);

                var result = _mapper.Map<ProjectDto>(project);
                return Result<ProjectDto>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<ProjectDto>(ex);
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
                return LogError<bool>(ex);
            }
        }

        private Result<T> LogError<T>(Exception ex, string message = "Service call failed", [CallerMemberName] string callerMemberName = "")
        {
            var msg = this.Caller(message, callerMemberName);
            _logger.LogError(ex, msg);
            return Result<T>.Failure(msg, ex);
        }
    }
}