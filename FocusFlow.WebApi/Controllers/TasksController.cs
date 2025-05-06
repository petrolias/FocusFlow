using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core.Services;
using FocusFlow.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ILogger<TasksController> logger, ITaskItemService taskItemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks([FromQuery] bool includeProjects = false)
        {
            try
            {
                var result = await taskItemService.GetAllAsync(includeProjects);
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
        public async Task<ActionResult<TaskItemDto>> GetTaskItem(Guid id, [FromQuery] bool includeTasks = false)
        {
            try
            {
                var result = await taskItemService.GetByIdAsync(id, includeTasks);
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
        public async Task<ActionResult> Create(TaskItemDtoBase taskItem)
        {
            try
            {
                var result = await taskItemService.AddAsync(taskItem, User.GetUserId());
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
        public async Task<IActionResult> Update(Guid id, TaskItemDtoBase taskItem)
        {
            try
            {
                var result = await taskItemService.UpdateAsync(id, taskItem, User.GetUserId());
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
                var result = await taskItemService.DeleteTaskItemAsync(id, User.GetUserId());
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