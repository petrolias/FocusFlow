using AutoMapper;
using FocusFlow.Core.Models;
using FocusFlow.WebApi.DTOs;

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
