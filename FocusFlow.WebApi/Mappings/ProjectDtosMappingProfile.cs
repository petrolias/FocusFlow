using AutoMapper;
using FocusFlow.Core.Models;
using FocusFlow.WebApi.DTOs;

namespace FocusFlow.WebApi.Mappings
{
    public class ProjectDtosMappingProfile : Profile
    {
        public ProjectDtosMappingProfile()
        {           
            CreateMap<ProjectDto, Project>();
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjecUpdateDto, Project>();
            CreateMap<Project, ProjectDto>();
        }
    }
}
