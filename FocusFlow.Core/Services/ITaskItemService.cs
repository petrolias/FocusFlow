using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;

namespace FocusFlow.Core.Services
{
    public interface ITaskItemService
    {
        Task<Result<TaskItemDto>> AddAsync(TaskItemDtoBase project, string userId);
        Task<Result<bool>> DeleteTaskItemAsync(Guid id, string userId);
        Task<Result<IEnumerable<TaskItemDto>>> GetAllAsync(bool includeProject = false);
        Task<Result<TaskItemDto?>> GetByIdAsync(Guid id, bool includeProject = false);
        Task<Result<IEnumerable<TaskItemDto>>> GetFilteredAsync(TaskItemFilter filter, bool includeProject = false);
        Task<Result<TaskItemDto>> UpdateAsync(Guid id, TaskItemDtoBase taskItem, string userId);
    }
}