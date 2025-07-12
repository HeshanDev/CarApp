# CarApp – Clean Architecture + Redis Caching Demo

This project demonstrates a simple .NET 8 Web API using **Clean Architecture**, **Domain-Driven Design (DDD)** principles, **Entity Framework Core with SQLite** for persistence, and **Redis** for distributed caching. It also uses the **Decorator Pattern** (via Scrutor) for cross-cutting concerns like caching.

---

## 🔧 Project Setup

### Create the solution and projects

```bash
dotnet new sln -n CarApp

# Create projects
dotnet new webapi -n CarApp.Api
dotnet new classlib -n CarApp.Application
dotnet new classlib -n CarApp.Domain
dotnet new classlib -n CarApp.Infrastructure
dotnet new classlib -n CarApp.Persistence

# Add projects to solution
dotnet sln add CarApp.Api/CarApp.Api.csproj
dotnet sln add CarApp.Application/CarApp.Application.csproj
dotnet sln add CarApp.Domain/CarApp.Domain.csproj
dotnet sln add CarApp.Infrastructure/CarApp.Infrastructure.csproj
dotnet sln add CarApp.Persistence/CarApp.Persistence.csproj

# Setup project references
dotnet add CarApp.Api reference CarApp.Application
dotnet add CarApp.Application reference CarApp.Domain
dotnet add CarApp.Application reference CarApp.Infrastructure
dotnet add CarApp.Infrastructure reference CarApp.Domain
dotnet add CarApp.Infrastructure reference CarApp.Persistence
dotnet add CarApp.Persistence reference CarApp.Domain
```


```bash
CarApp/
├── CarApp.Api/           → Web API entry point
├── CarApp.Application/   → Use cases, DTOs, Interfaces
├── CarApp.Domain/        → Entities, Enums, Interfaces
├── CarApp.Infrastructure/→ Redis caching, decorators, implementations
├── CarApp.Persistence/   → EF Core + SQLite
└── CarApp.sln            → Solution file
```