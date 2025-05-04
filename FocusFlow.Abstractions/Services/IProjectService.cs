using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;

namespace FocusFlow.Abstractions.Services
{
    public interface IProjectService
    {
        Task<Result<ProjectDto>> AddAsync(ProjectDtoBase project, string userId);
        Task<Result<bool>> DeleteProjectAsync(Guid id, string userId);
        Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(bool includeTask = false);
        Task<Result<ProjectDto?>> GetByIdAsync(Guid id, bool includeTask = false);
        Task<Result<IEnumerable<ProjectDto>>> GetFilteredAsync(ProjectFilter filter, bool includeTask = false);
        Task<Result<ProjectDto>> UpdateAsync(Guid id, ProjectDtoBase project, string userId);
    }
}