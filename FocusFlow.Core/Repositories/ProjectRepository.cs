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

        public async Task<Result<Project>> AddAsync(Project project)
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

        public async Task<Result<Project>> UpdateAsync(Project project)
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