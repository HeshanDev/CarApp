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

dotnet new xunit -n CarApp.Tests
dotnet sln add CarApp.Tests/CarApp.Tests.csproj
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
└── CarApp.Tests/         → Unit test project
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

### StackExchange.Redis

This is an infrastructure detail — a concrete technology to access Redis.

**Best place:** `CarApp.Infrastructure` project  
Because Infrastructure contains implementations for external systems (databases, caches, file systems, email, etc).

---

### Scrutor

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

###  Error handling

Add these NuGet packages in your API project:

```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Thread
dotnet add package Serilog.Enrichers.Process
dotnet add package Serilog.Enrichers.HttpContext

```


# Running and Testing CarApp .NET + Redis Docker Setup

## 1. Build Docker Images
From the root folder (where `docker-compose.yml` is located), run:


```bash
docker-compose build
```
## 2. Run Containers (App + Redis)

```bash
docker-compose up
```

## 3. Run Containers in Detached Mode
Run containers in the background:
```bash
docker-compose up -d
```

To follow logs after running in detached mode:
```bash
docker-compose logs -f
```

## 4. Check Running Containers
```bash
docker ps
```

## 5. Test Your API
Open a browser or API client (like Postman) and go to:
```bash
http://localhost:8080/swagger
```

## 6. View Logs
To view your API logs (when running in detached mode):
```bash
docker-compose logs -f carapp-api
```
To view Redis logs:
```bash
docker-compose logs -f redis
```

 ## 7. Stop Containers
 Stop and remove all containers:
```bash
 docker-compose down
```

  ## 8. Inspect Redis Data
```bash
  docker exec -it <redis_container_id_or_name> redis-cli
```
Additional Redis commands:
```bash
keys *
get <key>
```



