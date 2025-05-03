using AutoMapper;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
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
