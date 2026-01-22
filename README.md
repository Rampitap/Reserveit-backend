# Reserveit Backend

Backend API for a reservation management system for small businesses (business profiles, services, staff, reservations, availability slots, email notifications).

The project is built with **ASP.NET Core (C#)**, follows **Clean Architecture**, and uses **CQRS (MediatR)**.

---

## Features (high level)

- User authentication & authorization with **ASP.NET Identity**
- Clean Architecture layers (Domain / Application / Infrastructure / API)
- CQRS with **MediatR** (Commands/Queries)
- PostgreSQL database + EF Core migrations
- Reservations (client, staff, owner flows)
- Availability slots calculation
- Email notifications via **FluentEmail** + SMTP (real emails)
- Swagger API documentation
- Serilog logging

---

## Requirements

Install the following on your machine:

- **.NET SDK** (same major version as the solution uses)
- **Git**
- **Docker + Docker Compose** (recommended for PostgreSQL)
- *(Optional)* EF Core CLI tool: `dotnet-ef`

---

## Project Structure

Reserveit.Domain // Entities, enums, interfaces, exceptions, constants
Reserveit.Application // CQRS (MediatR), DTOs, validators, AutoMapper profiles
Reserveit.Infrastructure // EF Core DbContext, repositories, background workers, integrations
Reserveit.API // Controllers, middleware, DI, Swagger, logging

yaml
Copy code

---

## Quick Start (recommended)

### 1) Clone repository

```bash
git clone https://github.com/Rampitap/Reserveit-backend.git
cd Reserveit-backend
2) Start PostgreSQL (Docker)
From repository root:

bash
Copy code
docker compose up -d
Default docker-compose Postgres settings (expected by the project):

Host: localhost

Port: 5432

DB: reserveit_db

User: admin

Password: password123

If you change docker-compose credentials, update the connection string accordingly.

3) Restore packages (NuGet)
All packages are referenced in .csproj files, just run:

bash
Copy code
dotnet restore
4) Apply EF Core migrations
bash
Copy code
dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API
5) Run the API
bash
Copy code
dotnet run --project Reserveit.API
API default URLs (from launch settings):

HTTP: http://localhost:5054

HTTPS: https://localhost:7153

Swagger:

http://localhost:5054/swagger

https://localhost:7153/swagger

Configuration
Database (appsettings.json)
Default (Development):

json
Copy code
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=reserveit_db;Username=admin;Password=password123"
  }
}
If you ever run the API inside Docker too, change host to db (docker compose service name), not localhost.

Email (FluentEmail + Real SMTP) — IMPORTANT
This project sends real emails (not MailHog) for:

Reservation created (to client + staff)

Reservation status changed (to client + staff)

Reservation reminders (typically to client)

SMTP settings are stored in User Secrets
Do NOT put real SMTP credentials into appsettings.json.

Set up User Secrets (SMTP)
1) Initialize user-secrets
Run this in the Reserveit.API directory:

bash
Copy code
cd Reserveit.API
dotnet user-secrets init
2) Add SMTP credentials
Example for Gmail SMTP (recommended for development):

bash
Copy code
dotnet user-secrets set "Email:From" "********@gmail.com"
dotnet user-secrets set "Email:FromName" "Reserveit"
dotnet user-secrets set "Email:Smtp:Host" "smtp.gmail.com"
dotnet user-secrets set "Email:Smtp:Port" "587"
dotnet user-secrets set "Email:Smtp:User" "********@gmail.com"
dotnet user-secrets set "Email:Smtp:Password" "****************"
dotnet user-secrets set "Email:Smtp:UseSsl" "true"
Example secrets.json schema
Your secrets file will look like this (values hidden):

json
Copy code
{
  "Email": {
    "From": "********@gmail.com",
    "FromName": "Reserveit",
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": "587",
      "User": "********@gmail.com",
      "Password": "****************",
      "UseSsl": "true"
    }
  }
}
Where is secrets.json stored?
Windows: %APPDATA%\Microsoft\UserSecrets\<GUID>\secrets.json

Linux/macOS: ~/.microsoft/usersecrets/<GUID>/secrets.json

Gmail note: App Password is required
For Gmail you must use an App Password, not your normal Gmail password:

Enable 2-Step Verification

Go to Security → App passwords

Create an App Password (Mail / Other)

Use that generated password as Email:Smtp:Password

Applying changes to settings
Changes in appsettings.json are read normally.

Changes in user-secrets are used automatically in Development environment.

No extra “reload” is required — just restart the API.

Reset Database (when pulling new code)
If migrations or seed logic changed and something behaves weird, reset the DB.

Option A: reset via EF (recommended)
bash
Copy code
dotnet ef database drop --project Reserveit.Infrastructure --startup-project Reserveit.API
dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API
Option B: reset Docker volume (hard reset)
bash
Copy code
docker compose down -v
docker compose up -d
dotnet ef database update --project Reserveit.Infrastructure --startup-project Reserveit.API
Build & Publish
Build
bash
Copy code
dotnet build -c Release
Publish
bash
Copy code
dotnet publish Reserveit.API -c Release -o ./publish
Troubleshooting
EF tools warning (tools older than runtime)
If you see a message like “tools version older than runtime”, update dotnet-ef:

bash
Copy code
dotnet tool update --global dotnet-ef
Emails not sending (TLS/SSL errors)
If you get TLS errors with Gmail:

Use Port = 587

Ensure UseSsl = true

Ensure you are using App Password

Notes for Frontend Developers
Use Swagger to explore endpoints: /swagger

List endpoints typically support pagination (page, pageSize)

Filters can include date ranges and status for reservations

If you pull a newer backend version and categories/seed changes appear, reset DB
