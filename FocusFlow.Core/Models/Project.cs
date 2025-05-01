using FocusFlow.Abstractions.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FocusFlow.Core.Models
{
    public class Project : IEntityModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}