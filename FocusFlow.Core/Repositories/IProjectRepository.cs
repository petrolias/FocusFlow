using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Result<bool>> AddAsync(Project project);

        Task<Result<bool>> DeleteAsync(Guid id);

        Task<Result<IEnumerable<Project>>> GetAllAsync(bool includeTask = false);

        Task<Result<IEnumerable<Project>>> GetFilteredAsync(ProjectFilter filter, bool includeTask = false);

        Task<Result<Project?>> GetByIdAsync(Guid id, bool includeTask = false);

        Task<Result<bool>> UpdateAsync(Project project);
    }
}