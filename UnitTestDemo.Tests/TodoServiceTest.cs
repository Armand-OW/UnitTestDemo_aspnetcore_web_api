using System;
using Xunit;
using Moq;
using UnitTestDemoApplication.Services;
using UnitTestDemoApplication.Interfaces;
using UnitTestDemoApplication.Models;

namespace UnitTestDemo.Tests;

public class TodoServiceTest
{

    // ONE ISSUE WITH TESTS IN THIS SCENARIO
    // we need to mock the actual database
    private readonly Mock<ITodoRepository> _mockRepo;
    private readonly TodoService _service;
    public TodoServiceTest()
    {
        //this mimics my todo repository - database functionality (that I want to mock)
        _mockRepo = new Mock<ITodoRepository>();
        //using the mock repository in our services file that we are testing
        _service = new TodoService(_mockRepo.Object);
    }

    //naming convension of a test - name of the method + what we are testing

    // GETTING ITEM
    // Test GET the correct todo item
    [Fact]
    public async Task GetTodoByIdAsync_ReturnItem()
    {

        //Arrange
        //dummy item that we are returning
        TodoItem existingItem = new() { Id = 1, Title = "Existing Title", IsCompleted = true };
        //setting up our mock repository to return the existing item
        //1. setting up how we will call a function that we are mocking
        //2. specify what it is that we want the mock to return (existingItem)
        _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingItem);

        //identify the id that we want to use
        int ItemId = 1;

        //Act
        var result = await _service.GetTodoByIdAsync(ItemId);

        //Assert 
        //I'm checking if everything is what I expect it to be!
        Assert.NotNull(result); //item found
        Assert.Equal(existingItem, result); //making sure the objects match
        Assert.Equal("Existing Title", result.Title); //making sure title is correct
        Assert.True(result.IsCompleted); //making sure it is completed
        Assert.Equal(ItemId, result.Id); //making sure the id is correct

    }

    // Test input incorrect - item wasn't found (return null)

    [Fact]
    public async Task GetTodoByIdAsync_ReturnNull()
    {
        //Arrange
        _mockRepo
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((TodoItem?)null);

        int ItemId = 1234;

        //Act
        var result = await _service.GetTodoByIdAsync(ItemId);

        //Assert
        Assert.Null(result); //item not found
        Assert.NotEqual(ItemId, result?.Id); //making sure the id is not correct

    }


    //ADDING ITEM
    // Add an item, that it actually got added succesfully
    [Fact]
    public async Task AddTodoAsync_AddsItem()
    {
        // Arrange 
        // setting up our new to item that we want to add
        TodoItem newItem = new() { Title = "Test Adding", IsCompleted = false };
        // setting up our mock repository to return the new item
        _mockRepo.Setup(x => x.AddAsync(It.IsAny<TodoItem>())).ReturnsAsync(newItem);

        // Act
        // call the adding functionality
        var result = await _service.AddTodoAsync("Test Adding");

        // Assert
        // result is equal to our newItem
        Assert.Equal(newItem, result); //making sure the objects match
        Assert.Equal("Test Adding", result.Title); //making sure title is correct 
        Assert.False(result.IsCompleted); //making sure it is not completed
        Assert.NotNull(result); //making sure it has an id
        //Equal - 1. what we expect, 2. what we got
    }



    //TDD EXAMPLE: Test to change a todo item to completed/incompleted
    //1. Write the test first
    [Fact]
    public async Task ToggleTodoItemCompleteness_ReturnTheItem()
    {
        //Arrange
        //the item that was changed to true
        TodoItem changedTodoItem = new TodoItem { Id = 2, Title = "Changed Title", IsCompleted = true };
        //mock function
        _mockRepo.Setup(x => x.ToggleCompletionAsync(It.IsAny<int>())).ReturnsAsync(changedTodoItem);

        int itemId = 2;

        //Act
        var result = await _service.ToggleTodoByIdAsync(itemId);

        //Assert
        //checking if the correct item was found, and the toggle was done correctly as expected
        Assert.NotNull(result); //item found
        Assert.Equal(changedTodoItem, result); //making sure the objects match
        Assert.Equal("Changed Title", result.Title); //making sure title is correct
        Assert.True(result.IsCompleted); //making sure it is completed
        Assert.Equal(itemId, result.Id); //making sure the id is correct
    }

    //2. Write the code to make the test pass
}
