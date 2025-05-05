using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Services;
using FocusFlow.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(ILogger<ProjectsController> logger, IProjectService projectService) : ControllerBase
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
                logger.LogError(ex, message);
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
                logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProjectDtoBase project)
        {
            try
            {                
                var result = await projectService.AddAsync(project, User.GetUserId());
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProjectDtoBase project)
        {
            try
            {
                var result = await projectService.UpdateAsync(id, project, User.GetUserId());
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await projectService.DeleteProjectAsync(id, User.GetUserId());
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