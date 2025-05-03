using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.WebApi.DTOs;
using FocusFlow.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using TStartup = FocusFlow.WebApi.Program;
using FocusFlow.Core;

namespace FocusFlow.WebApi.Tests.Services
{
    public class ProjectServiceTests : IClassFixture<WebApiFactory<TStartup>>
    {
        private readonly Mock<IProjectRepository> _mockRepository;
        private readonly IProjectService _service;
      
        public ProjectServiceTests(WebApiFactory<TStartup> factory)
        {
            var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProjectService>>();
            
            _mockRepository = new Mock<IProjectRepository>();
            _service = new ProjectService(logger, mapper, _mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProjects()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), Name = "Test Project" } };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projects);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProject()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Name = "Test Project" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            // Act
            var result = await _service.GetByIdAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Test Project", result.Value.Name);
        }

        [Fact]
        public async Task AddAsync_AddsProject()
        {
            // Arrange
            var projectCreateDto = new ProjectCreateDto { Name = "New Project" };
            var project = new Project { Id = Guid.NewGuid(), Name = "New Project" };
            
            // Act
            var result = await _service.AddAsync(projectCreateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("New Project", result.Value.Name);
        }

        [Fact]
        public async Task UpdateProjectAsync_UpdatesProject()
        {
            // Arrange
            var projectUpdateDto = new ProjecUpdateDto { Name = "Updated Project" };
            var project = new Project { Id = Guid.NewGuid(), Name = "Updated Project" };            

            // Act
            var result = await _service.UpdateProjectAsync(projectUpdateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Updated Project", result.Value.Name);
        }

        [Fact]
        public async Task DeleteProjectAsync_DeletesProject()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAsync(projectId, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteProjectAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}