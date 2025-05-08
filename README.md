# FocusFlow – Task Management System

FocusFlow is a modular, task management application that allows authenticated users to manage projects and tasks with filtering, dashboards, and full CRUD operations. Built with a clean architecture in .NET and Blazor Server, it follows SOLID principles, uses Entity Framework Core, and supports containerization.

FocusFlow demonstrates:

- Layered architecture with strict separation of concerns (SOLID)
- Focus on testability and maintainability
- Use of .NET built-in frameworks (Identity, EF Core, Blazor)
- Containerization and health readiness for cloud deployment

💡 Design Decisions (ADR)

✅ Layered architecture for separation of concerns

✅ EF Core + repository pattern for data abstraction

✅ Result<T> pattern to encapsulate operation success/failure

✅ AutoMapper for model <=> DTO transformations

✅ Microsoft Identity for authentication/authorization

✅ JWT for Web API authentication

✅ Health checks and history tracking built-in

✅ ILogger used throughout for extensible logging

✅ No third-party packages — mostly native .NET

## Prerequisites

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

## Quick Start

## 🐳 Docker Support
Two Dockerfiles are available:

FocusFlow.WebApi/Dockerfile

FocusFlow.Blazor/Dockerfile
### Run with Docker Compose

```
docker-compose build
docker-compose up 
```
Visit: http://localhost:9090

username : admin@example.com

password : Admin123$

## 🌐 API Documentation (OpenAPI / Swagger)
OpenAPI documentation is available via Swagger UI:

Run the Web API

Visit: http://localhost:9091/swagger

## Migrations
Migrations are already included in the repository. But in case we need to recreate them here is the command.
``
dotnet ef migrations add InitialCreate --context Context --project ./FocusFlow.Core --startup-project ./FocusFlow.WebApi
``

# Architecture Overview

### Layered / Onion Architecture

graph TD
    A[Blazor (UI)] --> B[Web API]
    B --> C[Core Services]
    C --> D[EF Core Repositories]
    C --> E[Abstractions & DTOs]

## Project Structure

### FocusFlow.Abstraction 
(Abstraction Layer) Promotes decoupling and testability.
- Interfaces, 
- DTOs, and shared contracts used across all layers. 

Abstraction contains only interfaces, services interface and dto models that
are shared from core services to web api to blazor, + minor extension and common methods.
(Examples of Interfaces show knowlege of abstraction and 
generics use of c# in the code 
```
(<TModels> where Interface) examples)
```

### FocusFlow.Core
(Business logic Layer) 
EF Core, Repository pattern, Services, 
Validation (IsValid), Result<T> pattern, Logging ILogger<T>, AutoMapper, History Tables Tracking.

Uses Entity Framework Core (EF Core) to interact with the database. EF Core migrations are 
used to manage and evolve the database schema. 

Follows the repository pattern, where services use DTOs and rely on repositories for data access. 
Repositories handle model-to-DTO mapping via AutoMapper, keeping services decoupled from EF and ensuring clean separation of concerns.

Authentication is handled using Microsoft Identity with built-in identity tables. 

A database seeder initializes default data, and a central Dependencies class registers services for reuse in the Web API. 
Validation logic is enforced in repositories via IsValid (IValid interface) and adittional validations before creating or updating records.

The application uses ILogger consistently across all layers. This setup supports integration with third-party systems like Azure Logs, NLog, or Serilog. 

All IResult responses utilize ILogger for consistent logging.

AutoMapper with defined mapping profiles is used for model-to-DTO conversions. 
Transformation logic (model <=> DTO) is strictly confined to the service layer, maintaining separation of concerns.

Current implementation uses an SQLite connection provider, but can easily support all known DB solutions
due to Migrations. Tests are performed with in memory database.

Main Models include
- AppUser : IdentityUser, IModelGuid
- Project : ModelGuid, IModelGuid, IEntryBase, IValid
- TaskItem : ModelGuid, IModelGuid, IEntryBase, IValid

Project has many TaskItems
AppUser may be assigned to many TaskItems

Core services include:

- ProjectService
- TaskItemService
- UserService
- DashboardService

### FocusFlow.WebApi
(Communication layer)
Exposes RESTful endpoints with JWT authentication and Swagger UI. 
Health checks included for container support.

The Web API layer consumes the Core services and Abstraction layer, exposing them via RESTful endpoints. 
It uses JWT Bearer Tokens for authentication and includes Swagger (OpenAPI) for API documentation and testing.

The API exposes DTOs from the Core layer to external consumers and includes the following controllers:

- AuthController – handles user login and token issuance
- DashboardController – provides project/task statistics
- ProjectsController – full CRUD for projects
- TasksController – full CRUD for tasks
- UsersController – create, read, update users

Health Checks endpoints and probes (readiness, liveness etc), example of DatabaseHealthCheck, 
(to support deployment in container environemnts like Docker or Azure Container Apps).

### 🖥️ Frontend – FocusFlow.Blazor
Blazor Server UI layer with integration to the Web API via controller services.
Authentication uses JWT tokens to securely access backend resources.

All logic is isolated to support modularity and future service replacement.
This design allows the Web API to be replaced or extended independently, 
supporting future integration with other systems.

Authentication is handled using JWT tokens, allowing the Blazor app to securely access protected Web API endpoints.

Frontend controllers (acts mostly as proxy to WEBApi):

- AuthController - Token / Login
- DashboardController
- ProjectsController – CRUD operations
- TasksController – CRUD operations
- UsersController – Create, read, update

All .razor components interact through each own controllers. 
The UI was built <u>without</u> any third-party libraries, relying solely on native Blazor capabilities. 
HTML Tables (grids) support filtering, pagination, and navigation. 

Each main Component (e.g., projects, tasks) is split into:

1. A grid component for listing and interaction
2. A form component for create/update operations (using blazor validations)
3. Finally, a dashboard area presents key metrics and task/project statistics visually.

### FocusFlow.Tests
xUnit is used to test the service layers using in-memory EF Core database.
Uses mostly Assert.Equivalent(expected, actual) for result validation. 
Tests can be run using the following command from the solution root:

```
dotnet test
```

The test suite is built with xUnit and primarily uses Assert.Equivalent for result validation. 
It focuses on testing Core services using an in-memory EF Core database, without mocking the repository 
layer—ensuring realistic, end-to-end service behavior.

## Test Coverage

### DashboardServiceTests
✔️ GetProjectsStatsAsync_ReturnsStats

✔️ GetProjectsStatsAsync_ReturnsEmpty_WhenNoProjects

✔️ GetProjectsStatsAsync_ReturnsZeroCounts_WhenNoTasks

### ProjectServiceTests
✔️ GetAllAsync_ReturnsProjects

✔️ GetByIdAsync_ReturnsProject

✔️ AddAsync_AddsProject

✔️ UpdateProjectAsync_UpdatesProject

✔️ DeleteProjectAsync_DeletesProject

### TaskItemServiceTests

✔️ GetAllAsync_ReturnsTaskItems

✔️ GetByIdAsync_ReturnsTaskItem

✔️ AddAsync_AddsTaskItem

✔️ UpdateTaskItemAsync_UpdatesTaskItem

✔️ DeleteTaskItemAsync_DeletesTaskItem

### UserServiceTests
✔️ CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated

✔️ CreateUserAsync_Should_Not_Allow_Invalid_User

✔️ CreateUserAsync_Should_Not_Duplicate_User

✔️ CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated

✔️ AssignRoleToUserAsync_ShouldReturnSuccess_WhenRoleIsAssigned

> Note: Some filtering methods are tested but currently unused in Blazor UI.


## Other notes
## Migrations
Create the Migration
``
dotnet ef migrations add InitialCreate --context Context --project ./FocusFlow.Core --startup-project ./FocusFlow.WebApi
``
Apply the Migration
``
dotnet ef database update --context FocusFlowContext
``