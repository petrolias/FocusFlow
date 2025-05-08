using FocusFlow.Abstractions.Constants;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core;
using FocusFlow.Core.Models;
using FocusFlow.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    namespace FocusFlow.Tests.Tests.Services
    {
        public class DashboardServiceTests : IClassFixture<TestFixture>, IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly Context _context;
            private readonly IDashboardService _dashboardService;

            public DashboardServiceTests(TestFixture fixture)
            {
                _scope = fixture.ServiceProvider.CreateScope();
                _context = _scope.ServiceProvider.GetRequiredService<Context>();
                _dashboardService = _scope.ServiceProvider.GetRequiredService<IDashboardService>();

                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }

            public void Dispose() => _scope.Dispose();

            [Fact]
            public async Task GetProjectsStatsAsync_ReturnsStats()
            {                
                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Project",
                    Description = "For dashboard",
                    CreatedBy = "test-user",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    UpdatedBy = "test-user"
                };

                var now = DateTimeOffset.UtcNow;

                var tasks = new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "ToDo Task", Status = TaskItemStatusEnum.Todo, Project = project, CreatedBy = "test", CreatedAt = now, UpdatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Done Task", Status = TaskItemStatusEnum.Done, Project = project, CreatedBy = "test", CreatedAt = now, UpdatedAt = now },
                new() { Id = Guid.NewGuid(), Title = "Overdue Task", Status = TaskItemStatusEnum.InProgress, DueDate = now.AddDays(-2), Project = project, CreatedBy = "test", CreatedAt = now, UpdatedAt = now },
            };

                _context.Projects.Add(project);
                _context.TaskItems.AddRange(tasks);
                await _context.SaveChangesAsync();
                
                var result = await _dashboardService.GetProjectsStatsAsync();
                
                Assert.True(result.IsSuccess);
                var stats = result.Value.FirstOrDefault(s => s.ProjectId == project.Id);
                Assert.NotNull(stats);
                Assert.Equal(3, stats.Total);
                Assert.Equal(1, stats.Completed);
                Assert.Equal(1, stats.ToDo);
                Assert.Equal(1, stats.Overdue);
                Assert.Single(stats.ToDoTasks);
                Assert.Single(stats.OverdueTasks);
            }

            [Fact]
            public async Task GetProjectsStatsAsync_ReturnsEmpty_WhenNoProjects()
            {
                var result = await _dashboardService.GetProjectsStatsAsync();

                Assert.True(result.IsSuccess);
                Assert.Empty(result.Value);
            }

            [Fact]
            public async Task GetProjectsStatsAsync_ReturnsZeroCounts_WhenNoTasks()
            {
                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Empty Project",
                    Description = "No tasks",
                    CreatedBy = "user",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    UpdatedBy = "user"
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var result = await _dashboardService.GetProjectsStatsAsync();

                Assert.True(result.IsSuccess);
                var stat = result.Value.FirstOrDefault();
                Assert.NotNull(stat);
                Assert.Equal(0, stat.Total);
                Assert.Equal(0, stat.ToDo);
                Assert.Equal(0, stat.Overdue);
                Assert.Equal(0, stat.Completed);
            }
        }
    }
}