# CarApp – Clean Architecture + Redis Caching Demo

This project demonstrates a simple .NET 8 Web API using **Clean Architecture**, **Domain-Driven Design (DDD)** principles, **Entity Framework Core with SQLite** for persistence, and **Redis** for distributed caching. It also uses the **Decorator Pattern** (via Scrutor) for cross-cutting concerns like caching.

---

## 🔧 Initial project structure

### CLI commands use to create the solution and projects

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

## Folder structure
```bash
CarApp/
├── CarApp.Api/           → Web API entry point
├── CarApp.Application/   → Use cases, DTOs, Interfaces
├── CarApp.Domain/        → Entities, Enums, Interfaces
├── CarApp.Infrastructure/→ Redis caching, decorators, implementations
├── CarApp.Persistence/   → EF Core + SQLite
└── CarApp.sln            → Solution file
```

## Packages added
add this to CarApp.Api
```bash
dotnet add package Swashbuckle.AspNetCore
```

Add EF Core NuGet Packages
In CarApp.Persistence project, install:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Add the EF Core Design package to your startup project CarApp.Api

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```


To Create EF Core migrations and update DB
Install EF tools globally if not done:
```bash
dotnet tool install --global dotnet-ef
```

Create migrations (run in CarApp.Api folder):
```bash
dotnet ef migrations add InitialCreate --project ../CarApp.Persistence/CarApp.Persistence.csproj --startup-project .
dotnet ef database update --project ../CarApp.Persistence/CarApp.Persistence.csproj --startup-project .
```

## Distributed Caching with Redis + Decorator Pattern

Add Required NuGet Packages
In your CarApp.Infrastructure or 
CarApp.Api project (depending on where you want to configure caching), install:

```bash
dotnet add package StackExchange.Redis
dotnet add package Scrutor
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis

```

### Where to put Redis and Scrutor packages?

### 1️⃣ StackExchange.Redis

This is an infrastructure detail — a concrete technology to access Redis.

**Best place:** `CarApp.Infrastructure` project  
Because Infrastructure contains implementations for external systems (databases, caches, file systems, email, etc).

---

### 2️⃣ Scrutor

Scrutor is a DI helper library used during composition root setup (where you register services).

Since DI setup usually happens in the API project (or a dedicated Composition Root project),

**Best place:** `CarApp.Api` project  
— where you compose all dependencies together and wire them.




## To run and debug

###  Start Redis using Docker
Open terminal and run:

```bash
docker run -d --name redis-dev -p 6379:6379 redis:7.0-alpine
```

Optional: Check Redis Is Working

```bash
docker exec -it redis-dev redis-cli
```

```bash
keys *
```

```bash
get "your-cache-key"
```


To Stop Redis Container
```bash
docker stop redis-dev
docker rm redis-dev
```