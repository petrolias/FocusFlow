using FocusFlow.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger) : ControllerBase
    {        
        [HttpGet("project-stats")]
        public async Task<IActionResult> GetProjectsStats()
        {
            try
            {
                var result = await dashboardService.GetProjectsStatsAsync();
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }
    }
}