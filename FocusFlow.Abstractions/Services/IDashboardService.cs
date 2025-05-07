using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Common;

namespace FocusFlow.Abstractions.Services
{
    public interface IDashboardService
    {
        Task<Result<IEnumerable<ProjectTaskStatsDto>>> GetProjectsStatsAsync();
    }
}