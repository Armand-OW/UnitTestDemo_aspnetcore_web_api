using System;
using Microsoft.AspNetCore.Http.HttpResults;
using UnitTestDemoApplication.Data;
using UnitTestDemoApplication.Interfaces;
using UnitTestDemoApplication.Models;

namespace UnitTestDemoApplication.Services;
// replicating our endpoints
// similar to my controller - but without the endpoint functionality
/// <summary>
/// Service for managing todo items. Can add a new item, and get a specific item by id.
/// </summary>
public class TodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoItem> AddTodoAsync(string title)
    {
        var newItem = new TodoItem { Title = title, IsCompleted = false };
        return await _repository.AddAsync(newItem);
    }

    public async Task<TodoItem?> GetTodoByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TodoItem?> ToggleTodoByIdAsync(int id)
    {
        return await _repository.ToggleCompletionAsync(id);
    }
}
