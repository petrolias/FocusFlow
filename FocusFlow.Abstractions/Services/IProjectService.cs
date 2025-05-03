using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;

namespace FocusFlow.Abstractions.Services
{
    public interface IProjectService
    {
        Task<Result<ProjectDto>> AddAsync(ProjectCreateDto project);
        Task<Result<bool>> DeleteProjectAsync(Guid id);
        Task<Result<IEnumerable<ProjectDto>>> GetAllAsync();
        Task<Result<ProjectDto?>> GetByIdAsync(Guid id);
        Task<Result<ProjectDto>> UpdateProjectAsync(ProjecUpdateDto project);
    }
}