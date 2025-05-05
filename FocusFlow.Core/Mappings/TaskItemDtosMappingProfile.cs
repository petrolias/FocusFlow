using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
{
    public class TaskItemDtosMappingProfile : Profile
    {
        public TaskItemDtosMappingProfile()
        {           
            CreateMap<TaskItemDto, TaskItem>();
            CreateMap<TaskItemDtoBase, TaskItem>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItem, TaskItemDtoBase>();
        }
    }
}
