using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;

namespace FocusFlow.Abstractions.Services
{
    public interface IProjectService
    {
        Task<Result<ProjectDto>> AddAsync(ProjectDtoBase project, string userId);
        Task<Result<bool>> DeleteProjectAsync(Guid id, string userId);
        Task<Result<IEnumerable<ProjectDto>>> GetAllAsync();
        Task<Result<ProjectDto?>> GetByIdAsync(Guid id);
        Task<Result<ProjectDto>> UpdateProjectAsync(Guid id, ProjectDtoBase project, string userId);
    }
}