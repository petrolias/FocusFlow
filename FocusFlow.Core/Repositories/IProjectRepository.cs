using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Result<Project>> AddAsync(Project project);

        Task<Result<bool>> DeleteAsync(Guid id);

        Task<Result<IEnumerable<Project>>> GetAllAsync();

        Task<Result<Project?>> GetByIdAsync(Guid id);

        Task<Result<Project?>> GetByNameAsync(string projectName);

        Task<Result<Project>> UpdateAsync(Project project);
    }
}