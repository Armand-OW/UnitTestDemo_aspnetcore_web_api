using System;
using UnitTestDemoApplication.Data;
using UnitTestDemoApplication.Interfaces;
using UnitTestDemoApplication.Models;

namespace UnitTestDemoApplication.Services;

/// <summary>
/// Service for managing todo items. Can add a new item, and get a specific item by id.
/// </summary>
public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<TodoItem> AddAsync(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }
}
