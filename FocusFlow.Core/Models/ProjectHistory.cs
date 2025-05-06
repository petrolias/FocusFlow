using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class ProjectHistory : ModelGuid, IModelGuid, IHistory
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; } // "Modified" or "Deleted"
    }
}