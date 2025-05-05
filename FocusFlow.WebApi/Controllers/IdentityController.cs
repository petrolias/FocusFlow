using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FocusFlow.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = mapper.Map<AppUser>(registerDto);
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }       
    }
}