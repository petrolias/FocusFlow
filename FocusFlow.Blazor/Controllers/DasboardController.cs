using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(
     ILogger<DashboardController> logger,
     IHttpClientFactory httpClientFactory) : BaseController(httpClientFactory)
    {
        [HttpGet("project-stats")]
        public async Task<ActionResult<IEnumerable<ProjectTaskStatsDto>>> GetProjectStats()
        {
            try
            {
                var response = await GetHttpClient().GetAsync("api/dashboard/project-stats");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, await response.ReadErrorMessageAsync());

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectTaskStatsDto>>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred while fetching project stats.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }
    }
}
