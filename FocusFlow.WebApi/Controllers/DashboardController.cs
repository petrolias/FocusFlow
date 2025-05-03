//using FocusFlow.Core;
//using FocusFlow.Core.Constants;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace FocusFlow.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DashboardController(Context _context) : ControllerBase
//    {
//        // GET: api/dashboard/project/{projectId}
//        [HttpGet("project/{projectId}")]
//        public async Task<IActionResult> GetProjectStats(Guid projectId)
//        {
//            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();

//            if (!tasks.Any())
//                return NotFound("No tasks found for this project.");

//            var stats = new
//            {
//                ProjectId = projectId,
//                Total = tasks.Count,
//                Completed = tasks.Count(t => t.Status == TaskItemStatusEnum.Done),
//                Overdue = tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TaskItemStatusEnum.Done)
//            };

//            return Ok(stats);
//        }

//        // GET: api/dashboard/global
//        [HttpGet("global")]
//        public async Task<IActionResult> GetGlobalStats()
//        {
//            var tasks = await _context.Tasks.ToListAsync();

//            var stats = new
//            {
//                Total = tasks.Count,
//                Completed = tasks.Count(t => t.Status == TaskItemStatusEnum.Done),
//                Overdue = tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TaskItemStatusEnum.Done),
//                ActiveProjects = tasks.Select(t => t.ProjectId).Distinct().Count()
//            };

//            return Ok(stats);
//        }

//        // GET: api/dashboard/summary
//        [HttpGet("summary")]
//        public async Task<IActionResult> GetSummaryByProject()
//        {
//            var grouped = await _context.Tasks
//                .GroupBy(t => t.ProjectId)
//                .Select(g => new
//                {
//                    ProjectId = g.Key,
//                    Total = g.Count(),
//                    Completed = g.Count(t => t.Status == TaskItemStatusEnum.Done),
//                    Overdue = g.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TaskItemStatusEnum.Done)
//                }).ToListAsync();

//            return Ok(grouped);
//        }

//        // GET: api/dashboard/status-distribution
//        [HttpGet("status-distribution")]
//        public async Task<IActionResult> GetStatusDistribution()
//        {
//            var data = await _context.Tasks
//                .GroupBy(t => t.Status)
//                .Select(g => new
//                {
//                    Status = g.Key.ToString(),
//                    Count = g.Count()
//                })
//                .ToListAsync();

//            return Ok(data);
//        }

//        // GET: api/dashboard/avg-completion-time
//        [HttpGet("avg-completion-time")]
//        public async Task<IActionResult> GetAverageCompletionTime()
//        {
//            var completedTasks = await _context.Tasks
//                .Where(t => t.Status == TaskItemStatusEnum.Done && t.DueDate < DateTime.UtcNow)
//                .ToListAsync();

//            if (!completedTasks.Any())
//                return Ok("No completed tasks to evaluate.");

//            var average = completedTasks
//                .Select(t => (DateTime.UtcNow - t.DueDate).TotalDays)
//                .Average();

//            return Ok(new { AverageDaysOverdue = Math.Round(average, 2) });
//        }

//        // GET: api/dashboard/top-overdue?count=5
//        [HttpGet("top-overdue")]
//        public async Task<IActionResult> GetTopOverdueTasks([FromQuery] int count = 5)
//        {
//            var overdue = await _context.Tasks
//                .Where(t => t.Status != TaskItemStatusEnum.Done && t.DueDate < DateTime.UtcNow)
//                .OrderBy(t => t.DueDate)
//                .Take(count)
//                .Select(t => new
//                {
//                    t.Title,
//                    t.DueDate,
//                    DaysOverdue = (DateTime.UtcNow - t.DueDate).Days
//                })
//                .ToListAsync();

//            return Ok(overdue);
//        }

//        // GET: api/dashboard/user-tasks/{userId}
//        [HttpGet("user-tasks/{userId}")]
//        public async Task<IActionResult> GetTasksByUser(string userId)
//        {
//            var tasks = await _context.Tasks
//                .Where(t => t.AssignedUserId == userId)
//                .Select(t => new
//                {
//                    t.Title,
//                    t.Status,
//                    t.DueDate,
//                    t.Priority,
//                    t.ProjectId
//                })
//                .ToListAsync();

//            return Ok(new { UserId = userId, TaskCount = tasks.Count, Tasks = tasks });
//        }
//    }
//}