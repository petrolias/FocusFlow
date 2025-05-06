using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public partial class ProjectRepository(ILogger<ProjectRepository> logger, Context context)
        : IProjectRepository
    {
        private IQueryable<Project> Projects(bool includeTask = false)
        {
            var query = context.Projects.AsQueryable();
            if (includeTask)
                query = query.Include(p => p.Tasks);
            return query;
        }

        public async Task<Result<Project?>> GetByIdAsync(Guid id, bool includeTask = false)
        {
            try
            {
                Project? result = await Projects(includeTask).FirstOrDefaultAsync(p => p.Id == id);
                return Result<Project?>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<Project?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<Project>>> GetAllAsync(bool includeTask = false)
        {
            try
            {
                var result = await Projects(includeTask).ToListAsync();

                return Result<IEnumerable<Project>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<Project>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<Project>>> GetFilteredAsync(ProjectFilter filter, bool includeTask = false)
        {
            try
            {
                var query = Projects(includeTask);

                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(t => t.Name.ToLower().Contains(filter.Name.ToLower()));

                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    query = query
                        .Where(t => t.Description != null)
                        .Where(t => t.Description.ToLower().Contains(filter.Description.ToLower()));
                }

                var result = await query.ToListAsync();
                return Result<IEnumerable<Project>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<Project>>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> AddAsync(Project project)
        {
            try
            {
                if (project.Id == Guid.Empty)
                    project.Id = Guid.NewGuid();

                if (!project.IsValid(out var validationMessages))                    
                    return logger.FailureLog<bool>(LogLevel.Error, StatusCodes.Status400BadRequest,
                        $"Validations errors {string.Join(", ", validationMessages)} {nameof(project.Id)} : {project.Id}");
                
                var validationResult = await this.IsValidCreateAsync(project);
                if (!validationResult.IsSuccess)
                    return Result<bool>.From(validationResult);

                await context.Projects.AddAsync(project);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> UpdateAsync(Project project)
        {
            try
            {
                if (!project.IsValid(out var validationMessages))
                    return logger.FailureLog<bool>(LogLevel.Error, StatusCodes.Status400BadRequest,
                        $"Validations errors {string.Join(", ", validationMessages)} {nameof(project.Id)} : {project.Id}");

                var validationResult = await this.IsValidUpdateAsync(project);
                if (!validationResult.IsSuccess)
                    return Result<bool>.From(validationResult);

                context.Projects.Update(project);
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
                    context.Projects.Remove(getByIdResult.Value);
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