using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
{
    public class AuthDtosMappingProfile : Profile
    {
        public AuthDtosMappingProfile()
        {
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<AppUser, AppUserDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));          
        }
    }
}
