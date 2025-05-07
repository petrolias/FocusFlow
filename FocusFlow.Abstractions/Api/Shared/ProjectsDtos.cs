using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description can be at most 500 characters")]
        public string? Description { get; set; }
        public ICollection<TaskItemDto>? Tasks { get; set; } = [];        
    }
}