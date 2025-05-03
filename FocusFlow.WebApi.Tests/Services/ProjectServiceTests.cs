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
            var projectDto = new ProjectDtoBase { Name = "New Project", Description = "Description", CreatedBy = Guid.NewGuid().ToString() };            
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            var service = scope.ServiceProvider.GetRequiredService<IProjectService>();
            var result = await service.AddAsync(projectDto, projectDto.CreatedBy);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
                        
            Assert.Equal(projectDto.Name, result.Value.Name);
            Assert.Equal(projectDto.Description, result.Value.Description);
            Assert.Equal(projectDto.CreatedBy, result.Value.CreatedBy);
            Assert.NotNull(result.Value.CreatedAt);

            var actual = context.Projects.FirstOrDefault();
            Assert.Equal(projectDto.Name, actual.Name);
            Assert.Equal(projectDto.Description, actual.Description);
            Assert.Equal(projectDto.CreatedBy, actual.UpdatedBy);
            Assert.NotNull(result.Value.UpdatedAt);
        }

        [Fact]
        public async Task UpdateProjectAsync_UpdatesProject()
        {
            var projectDto = new ProjectDtoBase { Name = "Updated Project", Description = "Updated Description" };            
            var project = new Project { Id = Guid.NewGuid(), Name = "New Project", CreatedBy = Guid.NewGuid().ToString() };
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var service = scope.ServiceProvider.GetRequiredService<IProjectService>();
            var result = await service.UpdateAsync(project.Id, projectDto, project.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
           
            Assert.Equal(projectDto.Name, result.Value.Name);
            Assert.Equal(projectDto.Description, result.Value.Description);            
            Assert.Equal(project.CreatedBy, result.Value.UpdatedBy);

            var actual = context.Projects.FirstOrDefault();
            Assert.Equal(projectDto.Name, actual.Name);
            Assert.Equal(projectDto.Description, actual.Description);
            Assert.Equal(project.CreatedBy, actual.UpdatedBy);
            Assert.NotNull(result.Value.UpdatedAt);
        }

        [Fact]
        public async Task DeleteProjectAsync_DeletesProject()
        {
            var project = new Project { Id = Guid.NewGuid(), Name = "New Project", CreatedBy = Guid.NewGuid().ToString() };
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var service = scope.ServiceProvider.GetRequiredService<IProjectService>();
            var result = await service.DeleteProjectAsync(project.Id, project.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);            
            Assert.Empty(context.Projects);
        }
    }
}