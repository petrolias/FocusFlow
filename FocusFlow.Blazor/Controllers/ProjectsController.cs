using FocusFlow.Abstractions.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(
        ILogger<ProjectsController> logger,
        IHttpClientFactory httpClientFactory) : BaseController(httpClientFactory)
    {
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromQuery] bool includeTasks = false)
        {
            try
            {
                var response = await GetHttpClient().GetAsync($"api/projects?includeTasks={includeTasks}");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectDto>>();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(Guid id, [FromQuery] bool includeTasks = false)
        {
            try
            {
                var response = await GetHttpClient().GetAsync($"api/projects/{id}?includeTasks={includeTasks}");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<ProjectDto>();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, ProjectDtoBase project)
        {
            try
            {
                var response = await GetHttpClient().PutAsJsonAsync($"api/projects/{id}", project);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                
                var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Guid id, ProjectDtoBase project)
        {
            try
            {
                var response = await GetHttpClient().PostAsJsonAsync($"api/projects", project);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var response = await GetHttpClient().DeleteAsync($"api/projects/{id}");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                return Ok();
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