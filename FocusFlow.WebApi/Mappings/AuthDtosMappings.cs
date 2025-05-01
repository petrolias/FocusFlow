using FocusFlow.Core.Models;
using FocusFlow.WebApi.DTOs;

namespace FocusFlow.WebApi.Mappings
{
    public static class AuthDtosMappings
    {
        public static AppUser MapToAppUser(this RegisterDto dto) => new AppUser { UserName = dto.Username, Email = dto.Email };
    }
}
