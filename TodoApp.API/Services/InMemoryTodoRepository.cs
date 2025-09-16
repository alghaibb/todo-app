using TodoApp.API.Models;

namespace TodoApp.API.Services;

// Simple in-memory implementation to keep the assessment zero-setup.
// Notes to interviewer:
// - Fast to run and easy to review
// - Not persistent and not thread-safe (OK for demo; a DB-backed repo would use EF Core)
public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _todoItems = new();

    public InMemoryTodoRepository()
    {
        // Seed with a couple of items
        _todoItems.Add(new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Learn Angular",
            IsCompleted = false,
            CreatedAt = DateTime.Now.AddDays(-2)
        });

        _todoItems.Add(new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Build a Todo App",
            IsCompleted = false,
            CreatedAt = DateTime.Now.AddDays(-1)
        });
    }

    public IEnumerable<TodoItem> GetAll()
    {
        // Return newest first 
        return _todoItems.OrderByDescending(t => t.CreatedAt);
    }

    public TodoItem? GetById(Guid id) =>
        _todoItems.FirstOrDefault(t => t.Id == id);

    public TodoItem Add(TodoItem todoItem)
    {
        // Server-controlled fields: protect against over-posting.
        todoItem.Id = Guid.NewGuid();
        todoItem.CreatedAt = DateTime.Now;

        _todoItems.Add(todoItem);
        return todoItem;
    }

    public TodoItem? Update(TodoItem todoItem)
    {
        var existingItem = _todoItems.FirstOrDefault(t => t.Id == todoItem.Id);
        if (existingItem is null) return null;

        // Only updatable fields are changed; CreatedAt stays immutable.
        existingItem.Title = todoItem.Title;
        existingItem.IsCompleted = todoItem.IsCompleted;
        return existingItem;
    }

    public bool Delete(Guid id)
    {
        var existingItem = _todoItems.FirstOrDefault(t => t.Id == id);
        if (existingItem is null) return false;

        _todoItems.Remove(existingItem);
        return true;
    }
}
