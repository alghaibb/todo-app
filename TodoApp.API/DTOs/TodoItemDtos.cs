namespace TodoApp.API.DTOs;

public record TodoItemDto(Guid Id, string Title, bool IsCompleted, DateTime CreatedAt);

public record CreateTodoItemDto(string Title);

public record UpdateTodoItemDto(string Title, bool IsCompleted);
