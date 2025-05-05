using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class Project : ModelGuid, IModelGuid, IEntryBase
    {        
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}