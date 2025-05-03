using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Common;
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

        public async Task<Result<Project?>> GetByProjectNameAsync(string projectName)
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

        public async Task<IEnumerable<Project>> GetAllAsync() => await _context.Projects.Include(p => p.Tasks).ToListAsync();

        public async Task<Result<Project>> AddAsync(Project project, bool saveChanges)
        {
            try
            {
                if (project.Id == Guid.Empty)
                    project.Id = Guid.NewGuid();

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

        public async Task<Result<Project>> UpdateAsync(Project project, bool saveChanges)
        {
            try
            {
                var validationResult = await this.IsValidCreateAsync(project);
                if (!validationResult.IsSuccess)
                    return Result<Project>.From(validationResult);

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                return Result<Project>.Success(project);
            }
            catch (Exception ex)
            {
                return _logger.FailureLog<Project>(LogLevel.Error, exception: ex);
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, bool saveChanges)
        {
            try
            {
                var project = await GetByIdAsync(id);
                if (project != null)
                {
                    _context.Projects.Remove(project);
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