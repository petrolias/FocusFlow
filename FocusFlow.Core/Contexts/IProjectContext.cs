using FocusFlow.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusFlow.Core.Context
{
    public interface IContext<TEntityModel>
    {
        DbSet<Project> Projects { get; }
    }
}