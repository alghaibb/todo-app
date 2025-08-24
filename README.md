# Todo App

A simple Todo application with an Angular frontend and ASP.NET Core backend.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/) (v18 or later)
- [Angular CLI](https://angular.io/cli) (latest version)

## Setup & Running the Application

### Backend (ASP.NET Core API)

1. Navigate to the TodoApp.API directory:

   ```
   cd TodoApp.API
   ```

2. Restore the .NET packages:

   ```
   dotnet restore
   ```

3. Run the API:

   ```
   dotnet run
   ```

   The API will be available at `http://localhost:5169` and the Swagger UI at `http://localhost:5169/swagger`.

### Frontend (Angular)

1. Navigate to the TodoApp.Client directory:

   ```
   cd TodoApp.Client
   ```

2. Install the dependencies:

   ```
   npm install
   ```

3. Start the Angular development server:

   ```
   ng serve
   ```

   The Angular app will be available at `http://localhost:4200`.

## Features

- View list of Todo items
- Add new Todo items
- Mark Todo items as completed
- Delete Todo items

## Architecture

- **Frontend**: Angular 20 with standalone components and Angular Material
  - Uses the latest Angular CLI configuration with buildTarget property
  - Compatible with TypeScript 5.8 and zone.js 0.15
- **Backend**: ASP.NET Core 8 Web API with minimal APIs
- **Data Storage**: In-memory repository pattern (no database required)

## API Endpoints

- `GET /api/todos`: Get all todo items
- `GET /api/todos/{id}`: Get a specific todo item
- `POST /api/todos`: Create a new todo item
- `PUT /api/todos/{id}`: Update a todo item
- `DELETE /api/todos/{id}`: Delete a todo item
