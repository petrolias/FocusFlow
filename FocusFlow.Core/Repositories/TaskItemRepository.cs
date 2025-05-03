using FocusFlow.Core.Models;
using FocusFlow.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public class TaskItemRepository(ILogger<TaskItemRepository> _logger, Context _context) 
        : ITaskItemRepository
    {

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.TaskItems.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskItem> AddAsync(TaskItem taskItem, bool saveChanges)
        {
            if (taskItem.Id == Guid.Empty)
                taskItem.Id = Guid.NewGuid();

            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem taskItem, bool saveChanges)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task DeleteAsync(Guid id, bool saveChanges)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
