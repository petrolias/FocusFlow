using AutoMapper;
using FocusFlow.Abstractions.DTOs;
using FocusFlow.Abstractions.Services;
using FocusFlow.Core;
using FocusFlow.Core.Models;
using FocusFlow.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    public class ProjectServiceTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

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
                    CreatedBy = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Project {
                    Id = Guid.NewGuid(),
                    Name = "Project1",
                    Description = "Description1",
                    CreatedBy = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            }
            .Select(x=> { 
                x.UpdatedBy = x.CreatedBy; 
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
            var projectDto = new ProjectDtoBase { Name = "New Project", Description = "Description", CreatedBy = Guid.NewGuid().ToString() };
            var result = await _projectService.AddAsync(projectDto, projectDto.CreatedBy);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { projectDto.Name, projectDto.Description, projectDto.CreatedBy };
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

            var projectDto = new ProjectDtoBase { Name = "Updated Project", Description = "Updated Description", CreatedBy = project.CreatedBy };
            var result = await _projectService.UpdateAsync(project.Id, projectDto, project.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { projectDto.Name, projectDto.Description, projectDto.CreatedBy };
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
    }
}