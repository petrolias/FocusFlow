using System.ComponentModel.DataAnnotations;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class TaskItem : ModelGuid, IModelGuid, IEntryBase, IValid
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TaskItemStatusEnum? Status { get; set; }
        public TaskItemPriorityEnum? Priority { get; set; }
        public string? AssignedUserId { get; set; }
        public AppUser? AssignedUser { get; set; }
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public string GetUserInfo() => string.IsNullOrEmpty(AssignedUser?.Email) ? "Not available" : AssignedUser.Email;
        public bool IsValid(out List<string> validationErrors)
        {
            validationErrors = [];
            if (string.IsNullOrWhiteSpace(Title))
                validationErrors.Add("Title is required.");
            
            return !validationErrors.Any();
        }        
    }
}