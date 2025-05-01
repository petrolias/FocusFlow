using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocusFlow.Core.Constants;

namespace FocusFlow.Core.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        public string? AssignedUserId { get; set; }
        public AppUser? AssignedUser { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }
}
