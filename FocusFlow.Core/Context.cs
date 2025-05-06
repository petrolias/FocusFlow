using AutoMapper;
using FocusFlow.Abstractions.Models;
using FocusFlow.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FocusFlow.Core
{
    public class Context(DbContextOptions<Context> options, IMapper mapper) :
        IdentityDbContext<AppUser>(options)
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<ProjectHistory> ProjectHistories => Set<ProjectHistory>();
        public DbSet<TaskItemHistory> TaskItemHistories => Set<TaskItemHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId);

            modelBuilder.Entity<ProjectHistory>().HasKey(h => new { h.Id, h.ChangedAt });
            modelBuilder.Entity<TaskItemHistory>().HasKey(h => new { h.Id, h.ChangedAt });
        }

        public override int SaveChanges()
        {
            ProjectHistories.AddRange(GetHistoryEntries<Project, ProjectHistory>());
            TaskItemHistories.AddRange(GetHistoryEntries<TaskItem, TaskItemHistory>());
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProjectHistories.AddRange(GetHistoryEntries<Project, ProjectHistory>());
            TaskItemHistories.AddRange(GetHistoryEntries<TaskItem, TaskItemHistory>());
            return await base.SaveChangesAsync(cancellationToken);
        }

        private List<TModelHistory> GetHistoryEntries<TModel, TModelHistory>()
            where TModel : class
            where TModelHistory : class, IHistory
        {
            var entries = ChangeTracker
                .Entries<TModel>()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted);

            List<TModelHistory> modelHistories = [];
            foreach (var entry in entries)
            {
                var project = entry.Entity;
                var history = mapper.Map<TModelHistory>(project);
                history.ChangeType = entry.State.ToString();
                modelHistories.Add(history);
            }
            return modelHistories;
        }
    }
}