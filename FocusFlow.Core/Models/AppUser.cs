using FocusFlow.Abstractions.Models;
using Microsoft.AspNetCore.Identity;

namespace FocusFlow.Core.Models
{
    public class AppUser : IdentityUser, IEntityModel
    {
        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}