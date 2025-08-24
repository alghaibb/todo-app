using TodoApp.API.Models;

namespace TodoApp.API.Services;

public interface ITodoRepository
{
    IEnumerable<TodoItem> GetAll();
    TodoItem? GetById(Guid id);
    TodoItem Add(TodoItem todoItem);
    TodoItem? Update(TodoItem todoItem);
    bool Delete(Guid id);
}
