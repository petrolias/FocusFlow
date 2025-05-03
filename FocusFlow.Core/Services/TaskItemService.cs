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
    public class TaskItemService(
        ILogger<ProjectService> _logger,
        IMapper _mapper,
        ITaskItemRepository _taskItemRepository) : ITaskItemService
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
                return _logger.FailureLog<IEnumerable<TaskItemDto>>(LogLevel.Error, exception: ex);
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
                return _logger.FailureLog<TaskItemDto?>(LogLevel.Error, exception: ex);
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
                return _logger.FailureLog<IEnumerable<TaskItemDto>>(LogLevel.Error, exception: ex);
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
                return _logger.FailureLog<TaskItemDto>(LogLevel.Error, exception: ex);
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
                return _logger.FailureLog<TaskItemDto>(LogLevel.Error, exception: ex);
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
                return _logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }      
    }
}