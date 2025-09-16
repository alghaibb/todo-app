using TodoApp.API.Models;

namespace TodoApp.API.Services;

public interface ITodoRepository
{
    // Read all items. Kept IEnumerable to avoid leaking list specific ops.
    IEnumerable<TodoItem> GetAll();

    // Read by id. Returns null when not found (caller decides 404 vs other).
    TodoItem? GetById(Guid id);

    // Create: accepts a TodoItem (without Id/CreatedAt set by caller),
    // repository assigns Id and timestamps, then returns the created entity.
    TodoItem Add(TodoItem todoItem);

    // Update: returns the updated entity or null if not found.
    TodoItem? Update(TodoItem todoItem);

    // Delete by id: returns true if something was removed.
    bool Delete(Guid id);
}
