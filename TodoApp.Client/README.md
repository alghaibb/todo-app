# Todo App - Angular 20 Frontend

This README provides instructions for setting up and running the Angular 20 frontend for the Todo App.

## Prerequisites

- [Node.js](https://nodejs.org/en/) (v18 or later)
- [Angular CLI](https://angular.io/cli) (version 20.x)
- TypeScript 5.8 or later

## Setup Instructions

### Backend (ASP.NET Core API)

1. Navigate to the API project:

   ```bash
   cd TodoApp.API
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the API:
   ```bash
   dotnet run
   ```
   The API will be available at http://localhost:5169 and the Swagger UI at http://localhost:5169/swagger

### Frontend (Angular)

1. Navigate to the client project:

   ```bash
   cd TodoApp.Client
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the Angular development server:
   ```bash
   ng serve
   ```
   The Angular app will be available at http://localhost:4200

## Project Structure

### Backend (ASP.NET Core API)

- **Models**: Contains the `TodoItem` class
- **Services**: Contains the `ITodoRepository` interface and its in-memory implementation
- **DTOs**: Contains data transfer objects for API requests and responses
- **Program.cs**: Defines minimal API endpoints

### Frontend (Angular)

- **Models**: Defines TypeScript interfaces for Todo items
- **Services**: Contains the TodoService for API communication
- **Components**:
  - TodoFormComponent: For adding new Todo items
  - TodoListComponent: For displaying and managing Todo items

## Features

- View list of Todo items
- Add new Todo items
- Mark Todo items as completed/incomplete
- Delete Todo items

## API Endpoints

- `GET /api/todos`: Get all Todo items
- `GET /api/todos/{id}`: Get a specific Todo item
- `POST /api/todos`: Create a new Todo item
- `PUT /api/todos/{id}`: Update a Todo item
- `DELETE /api/todos/{id}`: Delete a Todo item
