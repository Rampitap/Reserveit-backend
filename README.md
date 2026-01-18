# Reserveit Backend

Backend API for a reservation management system for small businesses (services, staff, reservations, availability slots).

---

## Requirements

- **.NET SDK** (same major version as the solution)
- **Docker + Docker Compose** (recommended for PostgreSQL + MailHog)
- (Optional) **EF Core CLI tool**: `dotnet-ef`

---

## Quick Start (recommended)

### 1) Start infrastructure (PostgreSQL + MailHog)
From repository root:

bash
`docker compose up -d`
2) Restore dependencies (NuGet packages)
All packages are already referenced in .csproj. Just run:

bash
Copy code
`dotnet restore`
3) Apply database migrations
bash
Copy code
`dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API`
4) Run the API
bash
Copy code
`dotnet run --project Reserveit.API`
API will start on:

`HTTP: http://localhost:5054`

`HTTPS: https://localhost:7153`

Swagger:

`https://localhost:7153/swagger`

`http://localhost:5054/swagger`

Configuration (Development)
Database
Default connection string (Development):

json
Copy code
`"ConnectionStrings": {`
 ` "DefaultConnection": "Host=localhost;Port=5432;Database=reserveit_db;Username=admin;Password=password123"`
`}`
If you run API inside Docker too, the DB host should be db (service name in docker-compose), not localhost.

Email (FluentEmail + MailHog)
Development SMTP configuration:

json
Copy code
`"Email": {`
  `"From": "no-reply@reserveit.local",`
  `"FromName": "Reserveit",`
 `"Smtp": {`
  `  "Host": "localhost",`
   ` "Port": 1025,`
    `"User": "",`
  `  "Password": "",`
  `  "UseSsl": false`
 ` }`
`}`
MailHog:

SMTP: localhost:1025

Web UI: http://localhost:8025

EF Core CLI (dotnet-ef)
Check if installed:

bash
Copy code
`dotnet ef --version`
Install globally (if missing):

bash
Copy code
`dotnet tool install --global dotnet-ef`
Update:

bash
Copy code
`dotnet tool update --global dotnet-ef`
Reset Database (important when pulling new code)
Option A: reset via EF (recommended)
bash
Copy code
`dotnet ef database drop --project Reserveit.Infrastructure --startup-project Reserveit.API`
`dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API`
Option B: reset via Docker volume (removes all Postgres data)
bash
Copy code
`docker compose down -v`
`docker compose up -d`
`dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API`
Build
bash
Copy code
`dotnet build -c Release`
Publish (optional):

bash
Copy code
`dotnet publish Reserveit.API -c Release -o ./publish`
Architecture
Clean Architecture structure:

cpp
Copy code
Reserveit.Domain          // Entities, enums, interfaces, exceptions
Reserveit.Application     // CQRS (MediatR), DTOs, validators, mappings
Reserveit.Infrastructure  // EF Core, repositories, persistence, integrations
Reserveit.API             // Controllers, middleware, DI, Swagger, logging
Core patterns:

CQRS with MediatR

Manual validation using FluentValidation

Mapping via AutoMapper

Logging via Serilog

Auth via ASP.NET Identity 

Notes for Frontend Developers
Use Swagger to explore endpoints: /swagger

Pagination is used in list endpoints (page/pageSize).

If categories or seed data changed in the new version, reset DB (see above).
