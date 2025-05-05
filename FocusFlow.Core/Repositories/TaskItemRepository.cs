using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public partial class TaskItemRepository(ILogger<TaskItemRepository> logger, Context context) : ITaskItemRepository
    {
        private IQueryable<TaskItem> TaskItems(bool includeProject = false)
        {
            var query = context.TaskItems.AsQueryable();
            if (includeProject)
                query = query.Include(p => p.Project);
            return query;
        }

        public async Task<Result<TaskItem?>> GetByIdAsync(Guid id, bool includeProject = false)
        {
            try
            {
                var result = await TaskItems(includeProject).FirstOrDefaultAsync(t => t.Id == id);
                return Result<TaskItem?>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<TaskItem?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<TaskItem>>> GetAllAsync(bool includeProject = false)
        {
            try
            {
                var result = await TaskItems(includeProject).ToListAsync();

                return Result<IEnumerable<TaskItem>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<TaskItem>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<TaskItem>>> GetFilteredAsync(TaskItemFilter filter, bool includeProject = false)
        {
            try
            {
                var query = TaskItems(includeProject);

                if (filter.ProjectId.HasValue)
                    query = query.Where(t => t.ProjectId == filter.ProjectId.Value);

                if (!string.IsNullOrWhiteSpace(filter.Title))
                    query = query.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));

                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    query = query
                        .Where(t => t.Description != null)
                        .Where(t => t.Description.ToLower().Contains(filter.Description.ToLower()));
                }

                if (filter.Status != null)
                    query = query.Where(t => t.Status == filter.Status);

                if (filter.Priority != null)
                    query = query.Where(t => t.Priority == filter.Priority);

                if (filter.AssignedUserId != null)
                    query = query
                        .Where(t => t.AssignedUserId != null)
                        .Where(t => t.AssignedUserId.ToLower() == filter.AssignedUserId.ToLower());

                if (filter.DueDate.HasValue)
                    query = query
                       .Where(t => t.DueDate != null)
                       .Where(t => t.DueDate.Value.UtcDateTime.Date == filter.DueDate.Value.Date);

                if (filter.DueDateUntil.HasValue)
                    query = query
                       .Where(t => t.DueDate != null)
                       .Where(t => t.DueDate.Value.UtcDateTime.Date <= filter.DueDateUntil.Value.Date);

                var result = await query.ToListAsync();
                return Result<IEnumerable<TaskItem>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<TaskItem>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> AddAsync(TaskItem taskItem)
        {
            try
            {
                if (taskItem.Id == Guid.Empty)
                    taskItem.Id = Guid.NewGuid();

                var validationResult = await this.IsValidCreateAsync(taskItem);
                if (!validationResult.IsSuccess)
                    return Result<bool>.From(validationResult);

                await context.TaskItems.AddAsync(taskItem);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> UpdateAsync(TaskItem taskItem)
        {
            try
            {
                var validationResult = await this.IsValidUpdateAsync(taskItem);
                if (!validationResult.IsSuccess)
                    return Result<bool>.From(validationResult);

                context.TaskItems.Update(taskItem);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var getByIdResult = await GetByIdAsync(id);
                if (!getByIdResult.IsSuccess)
                    return Result<bool>.From(getByIdResult);

                if (getByIdResult.Value != null)
                {
                    context.TaskItems.Remove(getByIdResult.Value);
                    await context.SaveChangesAsync();
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }
    }
}