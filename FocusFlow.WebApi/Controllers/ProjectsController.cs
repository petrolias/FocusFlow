using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    //TODO add dto's add auto mapper here

    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(ILogger<ProjectsController> _logger, IProjectService projectService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var result = await projectService.GetAllAsync();
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                _logger.LogError(ex, this.Caller(message));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
        {
            try
            {
                var result = await projectService.GetByIdAsync(id);
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                _logger.LogError(ex, this.Caller(message));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProjectCreateDto project)
        {
            try
            {
                var result = await projectService.AddAsync(project);
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                _logger.LogError(ex, this.Caller(message));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProjecUpdateDto project)
        {
            try
            {
                var result = await projectService.UpdateProjectAsync(project);
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                _logger.LogError(ex, this.Caller(message));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await projectService.DeleteProjectAsync(id);
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred.";
                _logger.LogError(ex, this.Caller(message));
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }
    }
}