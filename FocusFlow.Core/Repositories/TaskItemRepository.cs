using FocusFlow.Abstractions.Repositories;
using FocusFlow.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusFlow.Core.Repositories
{
    public class TaskItemRepository(Context _context) : IBaseRepository<TaskItem>
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
