using TodoApp.API.Models;

namespace TodoApp.API.Services;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _todoItems = new();

    public InMemoryTodoRepository()
    {
        // Add some sample data
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
        return _todoItems.OrderByDescending(t => t.CreatedAt);
    }

    public TodoItem? GetById(Guid id)
    {
        return _todoItems.FirstOrDefault(t => t.Id == id);
    }

    public TodoItem Add(TodoItem todoItem)
    {
        todoItem.Id = Guid.NewGuid();
        todoItem.CreatedAt = DateTime.Now;
        
        _todoItems.Add(todoItem);
        
        return todoItem;
    }

    public TodoItem? Update(TodoItem todoItem)
    {
        var existingItem = _todoItems.FirstOrDefault(t => t.Id == todoItem.Id);
        
        if (existingItem == null)
        {
            return null;
        }
        
        existingItem.Title = todoItem.Title;
        existingItem.IsCompleted = todoItem.IsCompleted;
        
        return existingItem;
    }

    public bool Delete(Guid id)
    {
        var existingItem = _todoItems.FirstOrDefault(t => t.Id == id);
        
        if (existingItem == null)
        {
            return false;
        }
        
        _todoItems.Remove(existingItem);
        
        return true;
    }
}
