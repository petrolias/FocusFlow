namespace FocusFlow.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    //using Microsoft.AspNetCore.SignalR;
    using FocusFlow.Core.Models;
    using System;
    using FocusFlow.Core;
    using FocusFlow.Core.Constants;

    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(Context _context) : ControllerBase //IHubContext<TaskHub> hub
    {
        // GET: api/tasks?projectId=...&status=...&priority=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(
            [FromQuery] Guid? projectId,
            [FromQuery] TaskItemStatusEnum? status,
            [FromQuery] TaskItemPriorityEnum? priority)
        {
            var query = _context.Tasks.AsQueryable();

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority);

            return await query.Include(t => t.AssignedUser).ToListAsync();
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            return task is null ? NotFound() : task;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            //await _hub.Clients.All.SendAsync("TaskChanged", $"Created: {task.Title}");

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, TaskItem updated)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.DueDate = updated.DueDate;
            task.Status = updated.Status;
            task.Priority = updated.Priority;
            task.AssignedUserId = updated.AssignedUserId;
            task.ProjectId = updated.ProjectId;

            await _context.SaveChangesAsync();
            //await _hub.Clients.All.SendAsync("TaskChanged", $"Updated: {task.Title}");

            return NoContent();
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            //await _hub.Clients.All.SendAsync("TaskChanged", $"Deleted: {task.Title}");

            return NoContent();
        }
    }
}