using System.ComponentModel.DataAnnotations;
using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.Api.Shared
{
    public record TaskItemDtoBase 
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public Guid? ProjectId { get; set; }        
    };

    public record TaskItemDto : EntryRecordBase
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; }
        [StringLength(500, ErrorMessage = "Description can be at most 500 characters")]
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public TaskItemStatusEnum? Status { get; set; }
        [Required(ErrorMessage = "Priority is required")]
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public string? AssignedUserInfo { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectInfo() => string.IsNullOrEmpty(Project?.Name) ? "Not available" : Project.Name;
        public string DueDateFormatted() => DueDate == null || DueDate == DateTimeOffset.MinValue ? "Not available" : DueDate.Value.ToString("yyyy-MM-dd");
        public string? CreatedAtDateFormatted() => CreatedAt == DateTimeOffset.MinValue ? "Not available" : CreatedAt.ToString("yyyy-MM-dd");

        public ProjectDtoBase? Project { get; set; }
    }
}