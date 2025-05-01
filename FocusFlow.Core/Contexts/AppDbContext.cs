using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace FocusFlow.Core.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :
        IdentityDbContext<AppUser>(options)
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.TaskItems)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);
        }
    }
}

