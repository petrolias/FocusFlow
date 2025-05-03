using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TStartup = FocusFlow.WebApi.Program;
using FocusFlow.Core.Services;
using FocusFlow.Abstractions.Services;
using FocusFlow.WebApi.Tests;
using FocusFlow.Core;
using FocusFlow.Abstractions.Common;
using FocusFlow.Abstractions.DTOs;

namespace FocusFlow.Tests.Services
{
    public class ProjectServiceTests : IClassFixture<WebApiFactory<TStartup>>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProjectServiceTests(WebApiFactory<TStartup> factory)
        {
            _serviceScopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public (Mock<IProjectRepository> projectRepository, IProjectService projectService, IMapper mapper) GetMoqServices()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mockRepository = new Mock<IProjectRepository>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProjectService>>();
            var service = new ProjectService(logger, mapper, mockRepository.Object);
            return (mockRepository, service, mapper);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProjects()
        {
            var userId = Guid.NewGuid().ToString();
            var projects = new List<Project> { new Project {
                Id = Guid.NewGuid(),
                Name = "Project",
                Description = "Description",
                CreatedBy = userId,
                UpdatedBy = userId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
                }
            };
            var services = GetMoqServices();
            services.projectRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result<IEnumerable<Project>>.Success(projects));

            var result = await services.projectService.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
            var expected = services.mapper.Map<ProjectDto>(projects.First());
            Assert.Equivalent(expected, result.Value.First());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProject()
        {
            var userId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Project",
                Description = "Description",
                CreatedBy = userId,
                UpdatedBy = userId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            var services = GetMoqServices();
            services.projectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(Result<Project>.Success(project));

            var result = await services.projectService.GetByIdAsync(projectId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var expected = services.mapper.Map<ProjectDto>(project);
            Assert.Equivalent(expected, result.Value);
        }

        [Fact]
        public async Task AddAsync_AddsProject()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IProjectService>();

            var projectCreateDto = new ProjectDtoBase { Name = "New Project", Description = "Description" };
            var userId = Guid.NewGuid().ToString();
            var addResult = await service.AddAsync(projectCreateDto, userId);

            Assert.True(addResult.IsSuccess);
            Assert.NotNull(addResult.Value);

            var result = await service.GetByIdAsync(addResult.Value.Id);

            Assert.Equal(projectCreateDto.Name, result.Value.Name);
            Assert.Equal(projectCreateDto.Description, result.Value.Description);
            Assert.Equal(userId, result.Value.CreatedBy);
            Assert.NotNull(result.Value.CreatedAt);
        }

        //[Fact]
        //public async Task UpdateProjectAsync_UpdatesProject()
        //{
        //    var projectCreateDto = new ProjectDtoBase { Name = "New Project", Description = "Description" };
        //    var project = new Project { Id = Guid.NewGuid(), Name = "Updated Project" };

        //    // Act
        //    var result = await _service.UpdateProjectAsync(projectUpdateDto);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal("Updated Project", result.Value.Name);
        //}

        //[Fact]
        //public async Task DeleteProjectAsync_DeletesProject()
        //{
        //    // Arrange
        //    var projectId = Guid.NewGuid();
        //    _mockRepository.Setup(repo => repo.DeleteAsync(projectId, true)).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _service.DeleteProjectAsync(projectId);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //    Assert.True(result.Value);
        //}
    }
}