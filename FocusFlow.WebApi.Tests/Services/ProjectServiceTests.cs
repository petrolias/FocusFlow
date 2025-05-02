using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.WebApi.DTOs;
using FocusFlow.WebApi.Services;
using TStartup = FocusFlow.WebApi.Program;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.WebApi.Tests.Services
{
    public class ProjectServiceTests : IClassFixture<WebApiFactory<TStartup>>
    {
        private readonly Mock<ProjectRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ProjectService>> _mockLogger;
        private readonly ProjectService _service;


        public ProjectServiceTests(WebApiFactory<TStartup> factory)
        {            
            _client = factory.CreateClient();        
            _mockRepository = new Mock<ProjectRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ProjectService>>();
            _service = new ProjectService(_mockLogger.Object, _mockMapper.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProjects()
        {
            // Arrange
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), Name = "Test Project" } };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projects);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ProjectDto>>(projects)).Returns(new List<ProjectDto> { new ProjectDto { Name = "Test Project" } });

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
            _mockMapper.Setup(mapper => mapper.Map<ProjectDto>(project)).Returns(new ProjectDto { Name = "Test Project" });

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
            _mockMapper.Setup(mapper => mapper.Map<Project>(projectCreateDto)).Returns(project);
            _mockMapper.Setup(mapper => mapper.Map<ProjectDto>(project)).Returns(new ProjectDto { Name = "New Project" });

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
            var projectUpdateDto = new ProjecUpdatetDto { Name = "Updated Project" };
            var project = new Project { Id = Guid.NewGuid(), Name = "Updated Project" };
            _mockMapper.Setup(mapper => mapper.Map<Project>(projectUpdateDto)).Returns(project);
            _mockMapper.Setup(mapper => mapper.Map<ProjectDto>(project)).Returns(new ProjectDto { Name = "Updated Project" });

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