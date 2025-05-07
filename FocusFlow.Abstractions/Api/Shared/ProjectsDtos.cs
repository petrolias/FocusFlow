using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.Api.Shared
{
    public record ProjectDtoBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    };

    public record ProjectDto : EntryRecordBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<TaskItemDto>? Tasks { get; set; } = [];        
    }
}