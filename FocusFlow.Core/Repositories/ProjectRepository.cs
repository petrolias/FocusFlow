using FocusFlow.Abstractions.Repositories;
using FocusFlow.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusFlow.Core.Repositories
{
    internal class ProjectRepository : IBaseRepository<Project>
    {
        private readonly Context _context;

        public ProjectRepository(Context context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.Include(p => p.Tasks).ToListAsync();
        }

        public async Task AddAsync(Project project, bool saveChanges)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project, bool saveChanges)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, bool saveChanges)
        {
            var project = await GetByIdAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}