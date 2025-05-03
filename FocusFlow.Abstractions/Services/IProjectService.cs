using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;

namespace FocusFlow.Abstractions.Services
{
    public interface IProjectService
    {
        Task<Result<ProjectDto>> AddAsync(ProjectCreateDto project, string userId);
        Task<Result<bool>> DeleteProjectAsync(Guid id, string userId);
        Task<Result<IEnumerable<ProjectDto>>> GetAllAsync();
        Task<Result<ProjectDto?>> GetByIdAsync(Guid id);
        Task<Result<ProjectDto>> UpdateProjectAsync(ProjecUpdateDto project, string userId);
    }
}