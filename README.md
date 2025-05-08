FocusFlow – Task Management System

<b>FocusFlow</b> is a modular, task management application that allows authenticated users to manage projects and tasks with filtering, dashboards, and full CRUD operations. Built with a clean architecture in .NET and Blazor Server, it follows SOLID principles, uses Entity Framework Core, and supports containerization.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

---

## Quick Start

### ▶️ Run with Docker Compose

```bash
docker-compose build

docker-compose up 
```

Visit: http://localhost:9090
http://localhost:9090/

username : admin@example.com
username : Admin123$

## Migrations
Create the Migration

``
dotnet ef migrations add InitialCreate --context Context --project ./FocusFlow.Core --startup-project ./FocusFlow.WebApi
``

Apply the Migration

``
dotnet ef database update --context FocusFlowContext
``

## Docker
docker-compose build
docker-compose up

http://localhost:9090/

username : admin@example.com
username : Admin123$


T


🧠 Architecture Overview

Layered / Onion Architecture

graph TD
    A[Blazor (UI)] --> B[Web API]
    B --> C[Core Services]
    C --> D[EF Core Repositories]
    C --> E[Abstractions & DTOs]

🔹 Project Structure

FocusFlow.Abstraction
Interfaces, DTOs, and shared contracts used across all layers. Promotes decoupling and testability.

FocusFlow.Core
Business logic layer with EF Core, repository pattern, service classes, validation, logging, and history tracking.

FocusFlow.WebApi
Exposes RESTful endpoints with JWT authentication and Swagger UI. Health checks included for container support.

FocusFlow.Blazor
Blazor Server UI consuming the Web API via controller services. All logic is isolated to support modularity and future service replacement.

FocusFlow.Tests
xUnit tests for all service layers using in-memory EF Core database.

💡 Design Decisions (ADR)
✅ Layered architecture for separation of concerns

✅ EF Core + repository pattern for data abstraction

✅ Result<T> pattern to encapsulate operation success/failure

✅ AutoMapper for model <=> DTO transformations

✅ Microsoft Identity for authentication/authorization

✅ JWT for Web API authentication

✅ Health checks and history tracking built-in

✅ ILogger used throughout for extensible logging

✅ No third-party packages — 100% native .NET

Running Tests
Execute all tests from solution root:
dotnet test


🧾 Test Coverage
All tests are implemented in FocusFlow.Tests using xUnit and an in-memory EF Core provider. No repository mocking — full-stack service testing.

✔️ DashboardServiceTests
GetProjectsStatsAsync_ReturnsStats

GetProjectsStatsAsync_ReturnsEmpty_WhenNoProjects

GetProjectsStatsAsync_ReturnsZeroCounts_WhenNoTasks

✔️ ProjectServiceTests
GetAllAsync_ReturnsProjects

GetByIdAsync_ReturnsProject

AddAsync_AddsProject

UpdateProjectAsync_UpdatesProject

DeleteProjectAsync_DeletesProject

✔️ TaskItemServiceTests
GetAllAsync_ReturnsTaskItems

GetByIdAsync_ReturnsTaskItem

AddAsync_AddsTaskItem

UpdateTaskItemAsync_UpdatesTaskItem

DeleteTaskItemAsync_DeletesTaskItem

✔️ UserServiceTests
CreateUserAsync_ShouldReturnSuccess_WhenUserIsCreated

CreateUserAsync_Should_Not_Allow_Invalid_User

CreateUserAsync_Should_Not_Duplicate_User

CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated

AssignRoleToUserAsync_ShouldReturnSuccess_WhenRoleIsAssigned

Note: Some filtering methods are tested but currently unused in Blazor UI.

🌐 API Documentation (OpenAPI / Swagger)
OpenAPI documentation is available via Swagger UI:

Run the Web API

Visit: http://localhost:<port>/swagger

🖥️ Frontend – FocusFlow.Blazor
Blazor Server UI layer with full integration to the Web API via controller services.

Authentication uses JWT tokens to securely access backend resources.

Controllers:

AuthController

DashboardController

ProjectsController

TasksController

UsersController

Components:

Feature separation via GridComponent and FormComponent for CRUD operations.

Filtering, pagination, and navigation implemented.

Dashboard visualizes project and task stats.

💡 Design Choice:
All business logic for UI is channeled through services and controllers, enabling easy future replacement or API mocking.

🐳 Docker Support
Two Dockerfiles are available:

FocusFlow.WebApi/Dockerfile

FocusFlow.Blazor/Dockerfile

✅ Docker Compose Configuration
The Docker Compose setup exposes both apps internally and externally via port 9090:

📎 Summary
FocusFlow demonstrates:

🔸 Layered architecture with strict separation of concerns

🔸 Strong focus on testability and maintainability

🔸 Use of .NET built-in frameworks (Identity, EF Core, Blazor)

🔸 Containerization and health readiness for cloud deployment