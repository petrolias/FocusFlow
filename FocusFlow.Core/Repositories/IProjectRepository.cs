using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Result<Project>> AddAsync(Project project, bool saveChanges);

        Task<Result<bool>> DeleteAsync(Guid id, bool saveChanges);

        Task<IEnumerable<Project>> GetAllAsync();

        Task<Result<Project?>> GetByIdAsync(Guid id);

        Task<Result<Project?>> GetByProjectNameAsync(string projectName);

        Task<Result<Project>> UpdateAsync(Project project, bool saveChanges);
    }
}