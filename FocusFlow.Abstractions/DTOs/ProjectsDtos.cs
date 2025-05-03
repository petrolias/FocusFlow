using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.DTOs
{
    public record ProjectDtoBase : EntryRecordBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    };

    public record ProjectDto : ProjectDtoBase
    {
        public Guid Id { get; set; }
    }
}