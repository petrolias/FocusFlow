using AutoMapper;
using FocusFlow.Abstractions.Api.Shared;
using FocusFlow.Abstractions.Constants;
using FocusFlow.Core;
using FocusFlow.Core.Models;
using FocusFlow.Core.Services;
using FocusFlow.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace FocusFlow.Tests.Tests.Services
{
    public class TaskItemServiceTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ITaskItemService _taskItemService;

        public TaskItemServiceTests(TestFixture fixture)
        {
            _scope = fixture.ServiceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<Context>();
            _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
            _taskItemService = _scope.ServiceProvider.GetRequiredService<ITaskItemService>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void Dispose() => _scope.Dispose(); // Clean up the scope after each test

        [Fact]
        public async Task GetAllAsync_ReturnsTaskItems()
        {
            // Arrange
            var taskItems = new List<TaskItem> {
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    Description = "Description 1",
                    Status = TaskItemStatusEnum.Todo,
                    Priority = TaskItemPriorityEnum.Medium,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    Description = "Description 2",
                    Status = TaskItemStatusEnum.InProgress,
                    Priority = TaskItemPriorityEnum.High,
                    DueDate = DateTime.UtcNow.AddDays(3),
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
            _context.TaskItems.AddRange(taskItems);
            await _context.SaveChangesAsync();

            var result = await _taskItemService.GetAllAsync();
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            var expected = _mapper.Map<List<TaskItemDto>>(taskItems);

            Assert.Equivalent(expected, result.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTaskItem()
        {
            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Task",
                Description = "Description",
                Status = TaskItemStatusEnum.InProgress,
                Priority = TaskItemPriorityEnum.High,
                DueDate = DateTime.UtcNow.AddDays(5),
                CreatedBy = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            taskItem.UpdatedBy = taskItem.CreatedBy;

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var result = await _taskItemService.GetByIdAsync(taskItem.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var expected = _mapper.Map<TaskItemDto>(taskItem);
            Assert.Equivalent(expected, result.Value);
        }

        [Fact]
        public async Task AddAsync_AddsTaskItem()
        {
            var CreatedBy = Guid.NewGuid().ToString();
            var taskItemDto = new TaskItemDtoBase
            {
                Title = "New Task",
                Description = "Description",
                Status = TaskItemStatusEnum.Todo,
                Priority = TaskItemPriorityEnum.High,
                DueDate = DateTime.UtcNow.AddDays(7)
            };
            var result = await _taskItemService.AddAsync(taskItemDto, CreatedBy);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { taskItemDto.Title, taskItemDto.Description, taskItemDto.Status, taskItemDto.Priority, taskItemDto.DueDate, CreatedBy };
            var actual = new { result.Value.Title, result.Value.Description, result.Value.Status, result.Value.Priority, result.Value.DueDate, result.Value.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.True(result.Value.CreatedAt > DateTimeOffset.MinValue);

            var actualDb = _context.TaskItems.First();
            actual = new { actual.Title, actual.Description, actual.Status, actual.Priority, actual.DueDate, actual.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.True(result.Value.CreatedAt > DateTimeOffset.MinValue);
            Assert.True(actualDb.CreatedAt > DateTimeOffset.MinValue);
        }

        [Fact]
        public async Task UpdateTaskItemAsync_UpdatesTaskItem()
        {
            var CreatedBy = Guid.NewGuid().ToString();
            var taskItem = new TaskItem { Id = Guid.NewGuid(), Title = "New Task", CreatedBy = CreatedBy };
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var taskItemDto = new TaskItemDtoBase
            {
                Title = "Updated Task",
                Description = "Updated Description",
                Status = TaskItemStatusEnum.InProgress,
                Priority = TaskItemPriorityEnum.Low,
                DueDate = DateTime.UtcNow.AddDays(3)
            };

            var result = await _taskItemService.UpdateAsync(taskItem.Id, taskItemDto, taskItem.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var expected = new { taskItemDto.Title, taskItemDto.Description, taskItemDto.Status, taskItemDto.Priority, taskItemDto.DueDate, CreatedBy };
            var actual = new { result.Value.Title, result.Value.Description, result.Value.Status, result.Value.Priority, result.Value.DueDate, result.Value.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.Equal(result.Value.UpdatedBy, taskItem.CreatedBy);
            Assert.True(result.Value.UpdatedAt > DateTimeOffset.MinValue);

            var actualDb = _context.TaskItems.First();
            actual = new { actual.Title, actual.Description, actual.Status, actual.Priority, actual.DueDate, actual.CreatedBy };
            Assert.Equivalent(expected, actual);
            Assert.Equal(actualDb.UpdatedBy, taskItem.CreatedBy);
            Assert.True(actualDb.UpdatedAt > DateTimeOffset.MinValue);
        }

        [Fact]
        public async Task DeleteTaskItemAsync_DeletesTaskItem()
        {
            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "New Task",
                Status = TaskItemStatusEnum.Todo,
                Priority = TaskItemPriorityEnum.Low,
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedBy = Guid.NewGuid().ToString()
            };
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var result = await _taskItemService.DeleteTaskItemAsync(taskItem.Id, taskItem.CreatedBy);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            Assert.Empty(_context.TaskItems);
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsFilteredTaskItems()
        {
            var taskItems = new List<TaskItem> {
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "High Priority Task",
                    Description = "Description 1",
                    Status = TaskItemStatusEnum.Todo,
                    Priority = TaskItemPriorityEnum.High,
                    DueDate = DateTime.UtcNow.AddDays(2),
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Medium Priority Task",
                    Description = "Description 2",
                    Status = TaskItemStatusEnum.InProgress,
                    Priority = TaskItemPriorityEnum.Medium,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Low Priority Task",
                    Description = "Description 3",
                    Status = TaskItemStatusEnum.Done,
                    Priority = TaskItemPriorityEnum.Low,
                    DueDate = DateTime.UtcNow.AddDays(10),
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
            _context.TaskItems.AddRange(taskItems);
            await _context.SaveChangesAsync();

            // Test status filter
            var statusFilter = new TaskItemFilter { Status = TaskItemStatusEnum.Todo };
            var statusResult = await _taskItemService.GetFilteredAsync(statusFilter);
            Assert.True(statusResult.IsSuccess);
            Assert.Single(statusResult.Value);
            Assert.Equal(TaskItemStatusEnum.Todo, statusResult.Value.First().Status);

            // Test priority filter
            var priorityFilter = new TaskItemFilter { Priority = TaskItemPriorityEnum.High };
            var priorityResult = await _taskItemService.GetFilteredAsync(priorityFilter);
            Assert.True(priorityResult.IsSuccess);
            Assert.Single(priorityResult.Value);
            Assert.Equal(TaskItemPriorityEnum.High, priorityResult.Value.First().Priority);

            // Test due date filter
            var dueDateFilter = new TaskItemFilter { DueDate = DateTime.UtcNow.AddDays(2) };
            var dueDateResult = await _taskItemService.GetFilteredAsync(dueDateFilter);
            Assert.True(dueDateResult.IsSuccess);
            Assert.Single(dueDateResult.Value);
            Assert.Equal("High Priority Task", dueDateResult.Value.First().Title);

            // Test due date until filter
            var dueDateUntilFilter = new TaskItemFilter { DueDateUntil = DateTime.UtcNow.AddDays(5) };
            var dueDateUntilResult = await _taskItemService.GetFilteredAsync(dueDateUntilFilter);
            Assert.True(dueDateUntilResult.IsSuccess);
            Assert.Equal(2, dueDateUntilResult.Value.Count());
            Assert.Contains("High Priority Task", dueDateUntilResult.Value.Select(x => x.Title));
            Assert.Contains("Medium Priority Task", dueDateUntilResult.Value.Select(x => x.Title));

            // Test combined filters
            var combinedFilter = new TaskItemFilter
            {
                Status = TaskItemStatusEnum.InProgress,
                Priority = TaskItemPriorityEnum.Medium,
                DueDate = DateTime.UtcNow.AddDays(5)
            };
            var combinedResult = await _taskItemService.GetFilteredAsync(combinedFilter);
            Assert.True(combinedResult.IsSuccess);
            Assert.Single(combinedResult.Value);
            Assert.Equal("Medium Priority Task", combinedResult.Value.First().Title);

            // Test empty filter
            var emptyFilter = new TaskItemFilter();
            var emptyResult = await _taskItemService.GetFilteredAsync(emptyFilter);
            Assert.True(emptyResult.IsSuccess);
            Assert.Equal(3, emptyResult.Value.Count());
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsEmptyList_WhenNoMatch()
        {
            var taskItems = new List<TaskItem> {
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    Description = "Description 1",
                    Status = TaskItemStatusEnum.Todo,
                    Priority = TaskItemPriorityEnum.Medium,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    Description = "Description 2",
                    Status = TaskItemStatusEnum.InProgress,
                    Priority = TaskItemPriorityEnum.High,
                    DueDate = DateTime.UtcNow.AddDays(3),
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
            _context.TaskItems.AddRange(taskItems);
            await _context.SaveChangesAsync();

            var filter = new TaskItemFilter { Title = "Non-existent task" };
            var result = await _taskItemService.GetFilteredAsync(filter);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task GetAllAsync_WithEmptySearchTerm_ReturnsAllProjects()
        {
            var taskItems = new List<TaskItem> {
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    Description = "Description 1",
                    Status = TaskItemStatusEnum.Todo,
                    Priority = TaskItemPriorityEnum.Medium,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    CreatedBy = Guid.NewGuid().ToString()
                },
                new TaskItem {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    Description = "Description 2",
                    Status = TaskItemStatusEnum.InProgress,
                    Priority = TaskItemPriorityEnum.High,
                    DueDate = DateTime.UtcNow.AddDays(3),
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
            _context.TaskItems.AddRange(taskItems);
            await _context.SaveChangesAsync();

            var result = await _taskItemService.GetFilteredAsync(new());

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count());
        }
    }
}