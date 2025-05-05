using FocusFlow.Abstractions.Constants;

namespace FocusFlow.Abstractions.Api.Shared
{
    public class TaskItemFilter
    {
        public Guid? ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string AssignedUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DueDateUntil { get; set; }
    }
}