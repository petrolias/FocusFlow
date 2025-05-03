using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> AddAsync(TaskItem taskItem, bool saveChanges);
        Task DeleteAsync(Guid id, bool saveChanges);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(Guid projectId);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> UpdateAsync(TaskItem taskItem, bool saveChanges);
    }
}