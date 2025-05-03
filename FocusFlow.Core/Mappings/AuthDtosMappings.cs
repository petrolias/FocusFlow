using FocusFlow.Abstractions.DTOs;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
{
    public static class AuthDtosMappings
    {
        public static AppUser MapToAppUser(this RegisterDto dto) => new AppUser { UserName = dto.Username, Email = dto.Email };
    }
}
