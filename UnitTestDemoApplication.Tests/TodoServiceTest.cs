
using Moq;
using UnitTestDemoApplication.Models;
using UnitTestDemoApplication.Interfaces;
using UnitTestDemoApplication.Repositories;
using UnitTestDemoApplication.Services;
using Xunit;

namespace UnitTestDemoApplication.Tests;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _mockRepo;
    private readonly TodoService _service;

    public TodoServiceTests()
    {
        _mockRepo = new Mock<ITodoRepository>();
        _service = new TodoService(_mockRepo.Object);
    }

    [Fact]
    public async Task AddTodoAsync_ShouldReturnNewTodo()
    {
        // Arrange
        var newItem = new TodoItem { Id = 1, Title = "Test Task", IsCompleted = false };
        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
                 .ReturnsAsync(newItem);

        // Act
        var result = await _service.AddTodoAsync("Test Task");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnTodo_WhenFound()
    {
        // Arrange
        var item = new TodoItem { Id = 1, Title = "Existing Task", IsCompleted = false };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(item);

        // Act
        var result = await _service.GetTodoByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Existing Task", result.Title);
    }

    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((TodoItem?)null);

        // Act
        var result = await _service.GetTodoByIdAsync(99);

        // Assert
        Assert.Null(result);
    }
}
