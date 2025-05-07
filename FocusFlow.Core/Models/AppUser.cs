using FocusFlow.Abstractions.Models;
using Microsoft.AspNetCore.Identity;

namespace FocusFlow.Core.Models
{
    public class AppUser : IdentityUser, IModelGuid
    {
        public ICollection<TaskItem>? Tasks { get; set; } = [];
    }
}