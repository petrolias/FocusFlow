using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class TaskItem : Entry, IEntityModel, IEntry
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public TaskItemStatusEnum Status { get; set; }
        public TaskItemPriorityEnum Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public AppUser? AssignedUser { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }
}
