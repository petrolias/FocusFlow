using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController(UserManager<AppUser> userManager) : ControllerBase
    {
        [Authorize]
        [HttpGet("protected")] // GET: api/test/protected
        public async Task<IActionResult> GetProtectedResource()
        {
            var userName = User.Identity?.Name;
            if (userName == null)
                return Unauthorized();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { message = "You are authorized!", user });
        }
    }
}