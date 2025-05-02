using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FocusFlow.Core.Models;
using FocusFlow.Core.Repositories;
using FocusFlow.WebApi.Services;

namespace FocusFlow.WebApi.Tests.Services
{
    public class TaskItemServiceTests
    {
        private readonly Mock<TaskItemRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<TaskItemService>> _mockLogger;
        private readonly TaskItemService _service;

        public TaskItemServiceTests()
        {
            _mockRepository = new Mock<TaskItemRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TaskItemService>>();
            _service = new TaskItemService(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsTaskItems()
        {
            // Arrange
            var taskItems = new List<TaskItem> { new TaskItem { Id = Guid.NewGuid(), Name = "Test Task" } };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(taskItems);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTaskItem()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, Name = "Test Task" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(taskItem);

            // Act
            var result = await _service.GetByIdAsync(taskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Test Task", result.Value.Name);
        }

        [Fact]
        public async Task AddAsync_AddsTaskItem()
        {
            // Arrange
            var taskItem = new TaskItem { Id = Guid.NewGuid(), Name = "New Task" };
            _mockRepository.Setup(repo => repo.AddAsync(taskItem, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(taskItem);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("New Task", result.Value.Name);
        }

        [Fact]
        public async Task UpdateTaskItemAsync_UpdatesTaskItem()
        {
            // Arrange
            var taskItem = new TaskItem { Id = Guid.NewGuid(), Name = "Updated Task" };
            _mockRepository.Setup(repo => repo.UpdateAsync(taskItem, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateTaskItemAsync(taskItem);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Updated Task", result.Value.Name);
        }

        [Fact]
        public async Task DeleteTaskItemAsync_DeletesTaskItem()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAsync(taskId, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteTaskItemAsync(taskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }
    }
}