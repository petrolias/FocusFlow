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
            CreateMap<TaskItem, TaskItemHistory>()
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ChangeType, opt => opt.Ignore());
        }
    }
}
