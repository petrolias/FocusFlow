using FocusFlow.Abstractions.Api.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var client = httpClientFactory.CreateClient("ExternalApi");

            var response = await client.PostAsJsonAsync("/api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Login failed");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            Response.Cookies.Append("access_token", tokenResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok();
        }
    }

}
