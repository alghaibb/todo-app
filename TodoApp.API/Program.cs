var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register the TodoRepository service
builder.Services.AddSingleton<TodoApp.API.Services.ITodoRepository, TodoApp.API.Services.InMemoryTodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");

// Todo Endpoints
app.MapGet("/api/todos", (TodoApp.API.Services.ITodoRepository repository) =>
{
    var todos = repository.GetAll();
    var todoItemDtos = todos.Select(t => new TodoApp.API.DTOs.TodoItemDto(
        t.Id, t.Title, t.IsCompleted, t.CreatedAt
    ));
    
    return Results.Ok(todoItemDtos);
})
.WithName("GetAllTodos")
.WithOpenApi();

app.MapGet("/api/todos/{id}", (Guid id, TodoApp.API.Services.ITodoRepository repository) =>
{
    var todo = repository.GetById(id);
    
    if (todo == null)
    {
        return Results.NotFound();
    }
    
    var todoItemDto = new TodoApp.API.DTOs.TodoItemDto(
        todo.Id, todo.Title, todo.IsCompleted, todo.CreatedAt
    );
    
    return Results.Ok(todoItemDto);
})
.WithName("GetTodoById")
.WithOpenApi();

app.MapPost("/api/todos", (TodoApp.API.DTOs.CreateTodoItemDto createDto, TodoApp.API.Services.ITodoRepository repository) =>
{
    var todoItem = new TodoApp.API.Models.TodoItem
    {
        Title = createDto.Title,
        IsCompleted = false,
    };
    
    var created = repository.Add(todoItem);
    
    var todoItemDto = new TodoApp.API.DTOs.TodoItemDto(
        created.Id, created.Title, created.IsCompleted, created.CreatedAt
    );
    
    return Results.Created($"/api/todos/{created.Id}", todoItemDto);
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapPut("/api/todos/{id}", (Guid id, TodoApp.API.DTOs.UpdateTodoItemDto updateDto, TodoApp.API.Services.ITodoRepository repository) =>
{
    var existingItem = repository.GetById(id);
    
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    
    existingItem.Title = updateDto.Title;
    existingItem.IsCompleted = updateDto.IsCompleted;
    
    var updated = repository.Update(existingItem);
    
    if (updated == null)
    {
        return Results.NotFound();
    }
    
    var todoItemDto = new TodoApp.API.DTOs.TodoItemDto(
        updated.Id, updated.Title, updated.IsCompleted, updated.CreatedAt
    );
    
    return Results.Ok(todoItemDto);
})
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/api/todos/{id}", (Guid id, TodoApp.API.Services.ITodoRepository repository) =>
{
    var deleted = repository.Delete(id);
    
    if (!deleted)
    {
        return Results.NotFound();
    }
    
    return Results.NoContent();
})
.WithName("DeleteTodo")
.WithOpenApi();

app.Run();
