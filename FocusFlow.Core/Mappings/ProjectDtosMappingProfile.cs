using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
{
    public class ProjectDtosMappingProfile : Profile
    {
        public ProjectDtosMappingProfile()
        {           
            CreateMap<ProjectDto, Project>();
            CreateMap<ProjectDtoBase, Project>();            
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, ProjectDtoBase>();
        }
    }
}
