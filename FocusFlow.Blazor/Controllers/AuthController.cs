using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var response = await httpClientFactory.GetExternalTokenAsync(model);
            if (!response.IsSuccessStatusCode)
                return BadRequest("Login failed. Please try again.");

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return Ok(tokenResponse);
        }
    }
}