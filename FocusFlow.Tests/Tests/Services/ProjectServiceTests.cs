using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core;
using FocusFlow.Core.Models;
using FocusFlow.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    public class ProjectServiceTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public ProjectServiceTests(TestFixture fixture)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<Context>();
            _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
            _projectService = _scope.ServiceProvider.GetRequiredService<IProjectService>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void Dispose() => _scope.Dispose(); // Clean up the scope after each test

        [Fact]
        public async Task GetAllAsync_ReturnsProjects()
        {
            var projects = new List<Project> {
                new Project {
                    Id = Guid.NewGuid(),
                    Name = "Project",
                    Description = "Description",
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new Project {
                    Id = Guid.NewGuid(),
                    Name = "Project1",
                    Description = "Description1",
                    CreatedBy = Guid.NewGuid().ToString()
                }
            }
            .Select(x =>
            {
                x.UpdatedBy = x.CreatedBy;
                x.CreatedAt = DateTimeOffset.UtcNow;
                x.UpdatedAt = DateTimeOffset.UtcNow;
                return x;
            })
            .ToList();
            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();

            var result = await _projectService.GetAllAsync();
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            var expected = _mapper.Map<List<ProjectDto>>(projects);

            Assert.Equivalent(expected, result.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProject()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Project",
                Description = "Description",
                CreatedBy = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            project.UpdatedBy = project.CreatedBy;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var result = await _projectService.GetByIdAsync(project.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var expected = _mapper.Map<ProjectDto>(project);
            Assert.Equivalent(expected, result.Value);
        }

        [Fact]
        public async Task AddAsync_AddsProject()
        {
            var userId = Guid.NewGuid().ToString();
            var projectDto = new ProjectDtoBase { Name = "New Project", Description = "Description" };
            var result = await _projectService.AddAsync(projectDto, userId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { projectDto.Name, projectDto.Description, CreatedBy = userId };
            var actual = new { result.Value.Name, result.Value.Description, result.Value.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.True(result.Value.CreatedAt > DateTimeOffset.MinValue);

            var actualDb = _context.Projects.First();
            actual = new { actual.Name, actual.Description, actual.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.True(result.Value.CreatedAt > DateTimeOffset.MinValue);
            Assert.True(actualDb.CreatedAt > DateTimeOffset.MinValue);
        }

        [Fact]
        public async Task UpdateProjectAsync_UpdatesProject()
        {
            var project = new Project { Id = Guid.NewGuid(), Name = "New Project", CreatedBy = Guid.NewGuid().ToString() };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var projectDto = new ProjectDtoBase { Name = "Updated Project", Description = "Updated Description" };
            var result = await _projectService.UpdateAsync(project.Id, projectDto, project.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { projectDto.Name, projectDto.Description, project.CreatedBy };
            var actual = new { result.Value.Name, result.Value.Description, result.Value.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.Equal(result.Value.UpdatedBy, project.CreatedBy);
            Assert.True(result.Value.UpdatedAt > DateTimeOffset.MinValue);

            var actualDb = _context.Projects.First();
            actual = new { actual.Name, actual.Description, actual.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.Equal(actualDb.UpdatedBy, project.CreatedBy);
            Assert.True(actualDb.UpdatedAt > DateTimeOffset.MinValue);
        }

        [Fact]
        public async Task DeleteProjectAsync_DeletesProject()
        {
            var project = new Project { Id = Guid.NewGuid(), Name = "New Project", CreatedBy = Guid.NewGuid().ToString() };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var result = await _projectService.DeleteProjectAsync(project.Id, project.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            Assert.Empty(_context.Projects);
        }

        [Fact]
        public async Task GetAllAsync_WithFilter_ReturnsFilteredProjects()
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Project 1",
                    Description = "First test project development",
                    CreatedBy = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Development Project",
                    Description = "Second test project",
                    CreatedBy = Guid.NewGuid().ToString(),
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Another Project",
                    Description = "Project with development",
                    CreatedBy = Guid.NewGuid().ToString(),
                }
            }
            .Select(x =>
            {
                x.UpdatedBy = x.CreatedBy;
                x.CreatedAt = DateTimeOffset.UtcNow;
                x.UpdatedAt = DateTimeOffset.UtcNow;
                return x;
            })
            .ToList();

            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();

            // Test name filter
            var nameFilter = new ProjectFilter { Name = "Development" };
            var nameResult = await _projectService.GetFilteredAsync(nameFilter);
            Assert.True(nameResult.IsSuccess);
            Assert.Single(nameResult.Value);
            Assert.Contains(nameResult.Value, p => p.Name == "Development Project");

            // Test description filter
            var descFilter = new ProjectFilter { Description = "development" };
            var descResult = await _projectService.GetFilteredAsync(descFilter);
            Assert.True(descResult.IsSuccess);
            Assert.Equal(2, descResult.Value.Count());
            Assert.Contains(descResult.Value, p => p.Description.Contains("development", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsEmptyList_WhenNoMatch()
        {
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Project 1", Description = "Description 1", CreatedBy = Guid.NewGuid().ToString() },
                new Project { Id = Guid.NewGuid(), Name = "Project 2", Description = "Description 2", CreatedBy = Guid.NewGuid().ToString() }
            };
            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();
            var invalidFilter = new ProjectFilter { Name = "Nonexistent" };
            var result = await _projectService.GetFilteredAsync(invalidFilter);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task GetAllAsync_WithEmptySearchTerm_ReturnsAllProjects()
        {
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Project 1", Description = "Description 1", CreatedBy = Guid.NewGuid().ToString() },
                new Project { Id = Guid.NewGuid(), Name = "Project 2", Description = "Description 2", CreatedBy = Guid.NewGuid().ToString() }
            };
            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();

            var allResult = await _projectService.GetFilteredAsync(new());

            Assert.True(allResult.IsSuccess);
            Assert.Equal(2, allResult.Value.Count());
        }
    }
}