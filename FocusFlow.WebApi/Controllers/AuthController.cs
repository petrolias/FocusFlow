using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FocusFlow.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<AppUser> userManager, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginDto request)
        {
            var user = await userManager.FindByNameAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid username or password.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}