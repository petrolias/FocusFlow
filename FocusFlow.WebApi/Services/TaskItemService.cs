using System.Runtime.CompilerServices;
using AutoMapper;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.WebApi.DTOs;

namespace FocusFlow.WebApi.Services
{
    public class TaskItemService(
        ILogger<ProjectService> _logger,
        IMapper _mapper,
        TaskItemRepository _taskItemRepository) : ITaskItemService
    {
        public async Task<Result<IEnumerable<TaskItemDto>>> GetAllAsync()
        {
            try
            {
                var taskItems = await _taskItemRepository.GetAllAsync();
                if (!taskItems.Any())
                    return Result<IEnumerable<TaskItemDto>>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = _mapper.Map<IEnumerable<TaskItemDto>>(taskItems);
                return Result<IEnumerable<TaskItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<IEnumerable<TaskItemDto>>(ex);
            }
        }

        public async Task<Result<TaskItemDto?>> GetByIdAsync(Guid id)
        {
            try
            {
                var taskItem = await _taskItemRepository.GetByIdAsync(id);
                if (taskItem is null)
                    return Result<TaskItemDto?>.Failure(statusCode: StatusCodes.Status404NotFound);

                var result = _mapper.Map<TaskItemDto>(taskItem);
                return Result<TaskItemDto?>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<TaskItemDto?>(ex);
            }
        }

        public async Task<Result<IEnumerable<TaskItemDto>>> GetAllByProjectIdAsync(Guid projectId)
        {
            try
            {
                var taskItems = await _taskItemRepository.GetAllByProjectIdAsync(projectId);
                var result = _mapper.Map<IEnumerable<TaskItemDto>>(taskItems);
                return Result<IEnumerable<TaskItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<IEnumerable<TaskItemDto>>(ex);
            }
        }

        public async Task<Result<TaskItemDto>> AddAsync(TaskItemCreateDto taskItemCreateDto)
        {
            try
            {
                var taskItem = _mapper.Map<TaskItem>(taskItemCreateDto);
                await _taskItemRepository.AddAsync(taskItem, true);
                var result = _mapper.Map<TaskItemDto>(taskItem);
                return Result<TaskItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<TaskItemDto>(ex);
            }
        }

        public async Task<Result<TaskItemDto>> UpdateTaskItemAsync(TaskItemUpdateDto taskItemUpdateDto)
        {
            try
            {
                var taskItem = _mapper.Map<TaskItem>(taskItemUpdateDto);
                await _taskItemRepository.UpdateAsync(taskItem, true);
                var result = _mapper.Map<TaskItemDto>(taskItem);
                return Result<TaskItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                return LogError<TaskItemDto>(ex);
            }
        }

        public async Task<Result<bool>> DeleteTaskItemAsync(Guid id)
        {
            try
            {
                await _taskItemRepository.DeleteAsync(id, true);
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