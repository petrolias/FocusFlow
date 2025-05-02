using FocusFlow.Core.Constants;

namespace FocusFlow.WebApi.DTOs
{
    public record TaskItemCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
    };

    public record TaskItemUpdateDto : TaskItemDto
    {
    }

    public record TaskItemDto : TaskItemCreateDto
    {
        public Guid Id { get; set; }
    }
}