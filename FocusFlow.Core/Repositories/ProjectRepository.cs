using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Repositories
{
    public partial class ProjectRepository(ILogger<ProjectRepository> _logger, Context _context)
        : IProjectRepository
    {
        public async Task<Result<Project?>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);

                return Result<Project?>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<Project?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<Project?>> GetByNameAsync(string projectName)
        {
            try
            {
                var result = await _context.Projects
                 .Include(p => p.Tasks)
                 .FirstOrDefaultAsync(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));

                return Result<Project?>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<Project?>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<IEnumerable<Project>>> GetAllAsync() {
            try
            {
                var result = await _context.Projects.Include(p => p.Tasks).ToListAsync();

                return Result<IEnumerable<Project>>.Success(result);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<IEnumerable<Project>>(LogLevel.Error, exception: ex);
            }

          
        } 

        public async Task<Result<Project>> AddAsync(Project project, string userId)
        {
            try
            {
                if (project.Id == Guid.Empty)
                    project.Id = Guid.NewGuid();

                project.CreatedAt = DateTimeOffset.UtcNow;
                project.CreatedBy = userId;     

                var validationResult = await this.IsValidCreateAsync(project);
                if (!validationResult.IsSuccess)
                    return Result<Project>.From(validationResult);

                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
                return Result<Project>.Success(project);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<Project>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<Project>> UpdateAsync(Project project, string userId)
        {
            try
            {
                var validationResult = await this.IsValidCreateAsync(project);
                if (!validationResult.IsSuccess)
                    return Result<Project>.From(validationResult);

                var projectToUpdate = await _context.Projects.FirstAsync(p => p.Id == project.Id);

                projectToUpdate.UpdatedAt = DateTimeOffset.UtcNow;
                projectToUpdate.UpdatedBy = userId;
                projectToUpdate.Name = project.Name;
                projectToUpdate.Description = project.Description;                

                _context.Projects.Update(projectToUpdate);
                await _context.SaveChangesAsync();
                return Result<Project>.Success(projectToUpdate);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<Project>(LogLevel.Error, exception: ex);
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
                    _context.Projects.Remove(getByIdResult.Value);
                    await _context.SaveChangesAsync();
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<bool>(LogLevel.Error, exception: ex);
            }
        }
    }
}