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
        IHttpContextAccessor httpContextAccessor) : BaseController(httpClientFactory)
    {
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            try
            {
                var response = await GetHttpClient().GetAsync($"api/users");
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserDto>>();

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
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            try
            {
                var response = await httpClientFactory.ExternalApi().PostAsJsonAsync($"/api/users", model);
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