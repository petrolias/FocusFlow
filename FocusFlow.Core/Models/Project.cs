using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.Models;

namespace FocusFlow.Core.Models
{
    public class Project : ModelGuid, IModelGuid, IEntryBase, IValid
    {        
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; } = [];

        public bool IsValid(out List<string> validationErrors)
        {
            validationErrors = [];
            if (string.IsNullOrWhiteSpace(Name))
                validationErrors.Add("Name is required.");
                                    
            return !validationErrors.Any();
        }
    }
}