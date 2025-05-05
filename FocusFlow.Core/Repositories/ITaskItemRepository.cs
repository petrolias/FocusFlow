using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface ITaskItemRepository
    {
        Task<Result<bool>> AddAsync(TaskItem taskItem);

        Task<Result<bool>> DeleteAsync(Guid id);

        Task<Result<IEnumerable<TaskItem>>> GetAllAsync(bool includeProject = false);

        Task<Result<TaskItem?>> GetByIdAsync(Guid id, bool includeProject = false);

        Task<Result<IEnumerable<TaskItem>>> GetFilteredAsync(TaskItemFilter filter, bool includeProject = false);

        Task<Result<bool>> UpdateAsync(TaskItem taskItem);
    }
}