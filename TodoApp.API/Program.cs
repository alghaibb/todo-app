var builder = WebApplication.CreateBuilder(args);

// Swagger: makes it easy for reviewers to explore the API quickly.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: allow the Angular dev server to call this API during local development.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DI registration:
// Singleton is fine here because the backing store is a single in-memory List for the app lifetime.
// With a real DB context, we'd typically use AddScoped.
builder.Services.AddSingleton<TodoApp.API.Services.ITodoRepository, TodoApp.API.Services.InMemoryTodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI only in Development to keep prod surface minimal.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");

// ----------------- Endpoints -----------------

// GET /api/todos -> list all (newest first)
app.MapGet("/api/todos", (TodoApp.API.Services.ITodoRepository repository) =>
{
    var todos = repository.GetAll();

    // Map domain entity -> DTO (never leak internal types directly)
    var dto = todos.Select(t => new TodoApp.API.DTOs.TodoItemDto(
        t.Id, t.Title, t.IsCompleted, t.CreatedAt
    ));

    return Results.Ok(dto);
})
.WithName("GetAllTodos")
.WithOpenApi();

// GET /api/todos/{id} -> fetch one or 404
app.MapGet("/api/todos/{id}", (Guid id, TodoApp.API.Services.ITodoRepository repository) =>
{
    var todo = repository.GetById(id);
    if (todo is null) return Results.NotFound();

    var dto = new TodoApp.API.DTOs.TodoItemDto(todo.Id, todo.Title, todo.IsCompleted, todo.CreatedAt);
    return Results.Ok(dto);
})
.WithName("GetTodoById")
.WithOpenApi();

// POST /api/todos -> create and return 201 Created with location header
app.MapPost("/api/todos", (TodoApp.API.DTOs.CreateTodoItemDto createDto, TodoApp.API.Services.ITodoRepository repository) =>
{
    // Map DTO -> domain; server will set Id/CreatedAt.
    var entity = new TodoApp.API.Models.TodoItem
    {
        Title = createDto.Title,
        IsCompleted = false
    };

    var created = repository.Add(entity);

    var dto = new TodoApp.API.DTOs.TodoItemDto(created.Id, created.Title, created.IsCompleted, created.CreatedAt);

    // Results.Created sets the Location header to the new resource.
    return Results.Created($"/api/todos/{created.Id}", dto);
})
.WithName("CreateTodo")
.WithOpenApi();

// PUT /api/todos/{id} -> full update (id from route, fields from body)
app.MapPut("/api/todos/{id}", (Guid id, TodoApp.API.DTOs.UpdateTodoItemDto updateDto, TodoApp.API.Services.ITodoRepository repository) =>
{
    var existing = repository.GetById(id);
    if (existing is null) return Results.NotFound();

    // Apply allowed updates only.
    existing.Title = updateDto.Title;
    existing.IsCompleted = updateDto.IsCompleted;

    var updated = repository.Update(existing);
    if (updated is null) return Results.NotFound();

    var dto = new TodoApp.API.DTOs.TodoItemDto(updated.Id, updated.Title, updated.IsCompleted, updated.CreatedAt);
    return Results.Ok(dto);
})
.WithName("UpdateTodo")
.WithOpenApi();

// DELETE /api/todos/{id} -> 204 No Content on success, 404 if not found
app.MapDelete("/api/todos/{id}", (Guid id, TodoApp.API.Services.ITodoRepository repository) =>
{
    var deleted = repository.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteTodo")
.WithOpenApi();

app.Run();
