using AutoMapper;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.Core.Models;

namespace FocusFlow.Core.Mappings
{
    public class TaskItemDtosMappingProfile : Profile
    {
        public TaskItemDtosMappingProfile()
        {           
            CreateMap<TaskItemDto, TaskItem>();
            CreateMap<TaskItemCreateDto, TaskItem>();
            CreateMap<TaskItemUpdateDto, TaskItem>();
            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
