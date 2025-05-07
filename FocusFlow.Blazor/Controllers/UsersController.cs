using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(
        ILogger<UsersController> logger, 
        IHttpClientFactory httpClientFactory, 
        IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            try
            {
                var response = await httpClientFactory.ExternalAddUserAsync(model);
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