using FocusFlow.Core.Models;

namespace FocusFlow.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project, bool saveChanges);
        Task DeleteAsync(Guid id, bool saveChanges);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project?> GetByProjectNameAsync(string projectName);
        Task<Project> UpdateAsync(Project project, bool saveChanges);
    }
}