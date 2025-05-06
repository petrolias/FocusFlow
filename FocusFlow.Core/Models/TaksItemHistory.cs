using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class TaskItemHistory : ModelGuid, IModelGuid, IHistory
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public AppUser? AssignedUser { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; } // "Modified" or "Deleted"
    }
}