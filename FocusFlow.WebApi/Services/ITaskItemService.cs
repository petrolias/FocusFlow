using FocusFlow.Abstractions.Common;
using FocusFlow.WebApi.DTOs;

namespace FocusFlow.WebApi.Services
{
    public interface ITaskItemService
    {
        Task<Result<TaskItemDto>> AddAsync(TaskItemCreateDto taskItemCreateDto);
        Task<Result<bool>> DeleteTaskItemAsync(Guid id);
        Task<Result<IEnumerable<TaskItemDto>>> GetAllAsync();
        Task<Result<IEnumerable<TaskItemDto>>> GetAllByProjectIdAsync(Guid projectId);
        Task<Result<TaskItemDto?>> GetByIdAsync(Guid id);
        Task<Result<TaskItemDto>> UpdateTaskItemAsync(TaskItemUpdateDto taskItemUpdateDto);
    }
}