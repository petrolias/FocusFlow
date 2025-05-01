using FocusFlow.Core;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FocusFlow.WebApi.Controllers
{
    //TODO add dto's add auto mapper here

    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(Context _context) : ControllerBase
    {        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
            => await _context.Projects.Include(p => p.Tasks).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
            return project is null ? NotFound() : project;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Project updated)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project is null) return NotFound();

            project.Name = updated.Name;
            project.Description = updated.Description;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project is null) return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
