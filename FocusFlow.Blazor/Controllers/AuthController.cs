using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        ILogger<ProjectsController> logger,
        IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : BaseController(httpClientFactory)
    {
        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var response = await httpClientFactory.ExternalApi().PostAsJsonAsync($"/api/auth/token", model);                
                if (!response.IsSuccessStatusCode)
                    return BadRequest("Login failed. Please try again.");

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return Ok(tokenResponse);
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