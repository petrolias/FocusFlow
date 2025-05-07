using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;

namespace FocusFlow.Abstractions.Services
{
    public interface ITaskItemService
    {
        Task<Result<TaskItemDto>> AddAsync(TaskItemDtoBase project, string userId);
        Task<Result<bool>> DeleteAsync(Guid id, string userId);
        Task<Result<IEnumerable<TaskItemDto>>> GetAllAsync(bool includeProject = false);
        Task<Result<TaskItemDto?>> GetByIdAsync(Guid id, bool includeProject = false);
        Task<Result<IEnumerable<TaskItemDto>>> GetFilteredAsync(TaskItemFilter filter, bool includeProject = false);
        Task<Result<TaskItemDto>> UpdateAsync(Guid id, TaskItemDtoBase taskItem, string userId);
    }
}