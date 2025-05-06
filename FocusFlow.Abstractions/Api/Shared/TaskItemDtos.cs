using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.Api.Shared
{
    public record TaskItemDtoBase : EntryRecordBase
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public string? AssignedUserName{ get; set; }
        public Guid? ProjectId { get; set; }
    };

    public record TaskItemDto : TaskItemDtoBase
    {
        public Guid Id { get; set; }
    }
}