using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Core.Services
{
    public class DashboardService(
        ILogger<DashboardService> logger,
        IMapper mapper,
        IUserService userService,
        IProjectService projectService
        ) : IDashboardService
    {
        public async Task<Result<IEnumerable<ProjectTaskStatsDto>>> GetProjectsStatsAsync()
        {
            try
            {
                var result = new List<ProjectTaskStatsDto>();

                var projectResult = await projectService.GetAllAsync(true);
                if (!projectResult.IsSuccess)
                    return Result<IEnumerable<ProjectTaskStatsDto>>.From(projectResult);
               
                foreach (var item in projectResult.Value)
                {
                    var stats = new ProjectTaskStatsDto
                    {
                        ProjectId = item.Id,
                        ProjectName = item.Name,
                        Total = item.Tasks.Count(),
                        Completed = item.Tasks.Count(t => t.Status == TaskItemStatusEnum.Done),
                        OverdueTasks = item.Tasks.Where(t => t.DueDate < DateTime.UtcNow && t.Status != TaskItemStatusEnum.Done).ToList()
                    };
                    stats.Overdue = stats.OverdueTasks.Count();
                    result.Add(stats);
                }

                return Result<IEnumerable<ProjectTaskStatsDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return logger.FailureLog<IEnumerable<ProjectTaskStatsDto>>(LogLevel.Error, exception: ex);
            }
        }
    }
}