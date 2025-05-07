using FocusFlow.Abstractions.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(
        ILogger<ProjectsController> logger,
        IHttpClientFactory httpClientFactory) : BaseController(httpClientFactory)
    {
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks()
        {
            try
            {
                var response = await GetHttpClient().GetAsync($"api/tasks?includeProjects={true}");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<TaskItemDto>>();

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
        public async Task<ActionResult<TaskItemDto>> GetTaskById(Guid id)
        {
            try
            {
                var response = await GetHttpClient().GetAsync($"api/tasks/{id}?includeProjects={true}");
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
        public async Task<IActionResult> UpdateTask(Guid id, TaskItemDtoBase project)
        {
            try
            {
                var response = await GetHttpClient().PutAsJsonAsync($"api/tasks/{id}", project);
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

        [HttpPost]
        public async Task<IActionResult> CreateProject(Guid id, TaskItemDtoBase project)
        {
            try
            {
                var response = await GetHttpClient().PostAsJsonAsync($"api/tasks", project);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<TaskItemDtoBase>();
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
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var response = await GetHttpClient().DeleteAsync($"api/tasks/{id}");
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