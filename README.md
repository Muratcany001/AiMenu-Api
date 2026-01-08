# AiMenu API

A lightweight ASP.NET Core Web API for managing users, menu items and order items. The solution is organized into multiple projects (UI, Business Logic, Data Access, DTOs and shared components). It uses Entity Framework Core for persistence, FluentValidation for input validation, AutoMapper for mapping and JWT for authentication.

## Table of contents
- [Quick start](#quick-start)
- [Tech stack](#tech-stack)
- [Repository structure](#repository-structure)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [Database migrations](#database-migrations)
- [Build & run](#build--run)
- [API reference (important endpoints)](#api-reference-important-endpoints)
  - [Authentication / Users](#authentication--users)
  - [Menu items](#menu-items)
  - [Order items](#order-items)
- [Testing the API](#testing-the-api)
- [Development notes](#development-notes)
- [Troubleshooting](#troubleshooting)
- [Next steps & contribution](#next-steps--contribution)

---

## Quick start

1. Update the database connection string in `PD.UI/appsettings.json` (or use environment variables / user secrets).
2. Apply EF Core migrations (see below).
3. Run the API (Visual Studio or `dotnet run`) and use Swagger or the supplied HTTP examples to test endpoints.

---

## Tech stack
- .NET 7 / ASP.NET Core (Web API)
- Entity Framework Core (Code First)
- AutoMapper
- FluentValidation
- JWT for authentication
- SQL Server (connection string in appsettings.json)

---

## Repository structure

Top-level solution: `AiMenu.sln`

Important projects:
- `PD.UI` — API project (controllers, Program.cs, appsettings). Entry point.
- `PD.BL` — Business logic (services, validators, AutoMapper profiles, helpers).
- `PD.DAL` — Data access: EF Core `Context`, entities and migrations.
- `Dtos` — DTO classes used by API requests/responses.
- `Common` — shared view models (e.g., `ResultViewModel<T>`).

Key files:
- `PD.UI/Program.cs` — DI container, middlewares, service registrations.
- `PD.DAL/Context.cs` — EF Core DbContext and relationships.
- `PD.DAL/Migrations/*` — EF migrations already present.
- Controllers:
  - `PD.UI/Controllers/UserController.cs`
  - `PD.UI/Controllers/MenuItemController.cs`
  - `PD.UI/Controllers/OrderItemController.cs`
- Sample request file: `PD.UI/PD.UI.http`

---

## Prerequisites

- .NET SDK (version compatible with solution; typically .NET 7 or later)
- SQL Server instance (local or remote)
- (Optional) Visual Studio 2022/2023 or VS Code

---

## Configuration

Primary configuration is in `PD.UI/appsettings.json`:

Example (already in repo):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=DESKTOP-KP4MLAD\\murat;Database=AiMenu;Trusted_Connection=True;TrustServerCertificate=True;"
},
"JwtSettings": {
  "SecretKey": "super-guclu-bir-sifre-bu-olsun-deneme-123-abc-XYZ-!@#-6543214221",
  "Issuer": "https://localhost:7276",
  "Audience": "https://localhost:7276"
}
```

- Connection string: update `DefaultConnection` to point to your SQL Server instance.
- JWT secret: the repo contains a sample secret. For production, use secure storage (Azure Key Vault, environment variables, or user secrets).

Overriding configuration:
- Use environment variables (ASPNETCORE_ENVIRONMENT) or `dotnet user-secrets` for local secrets.
- You can also set `ASPNETCORE_URLS` to change the listening address.

---

## Database migrations

Migrations already exist under `PD.DAL/Migrations`. To apply them:

1. Install EF Core tools (if not already):
   - dotnet tool install --global dotnet-ef
2. From repository root (or `PD.DAL` directory), run:
   - dotnet ef database update --project PD.DAL --startup-project PD.UI

Notes:
- `--project PD.DAL` points to the migrations project.
- `--startup-project PD.UI` ensures the connection string and DbContext are picked from the API project.

If you prefer Visual Studio:
- Open solution -> Package Manager Console:
  - Default project: PD.DAL
  - Run: Update-Database

---

## Build & run

Using Visual Studio:
- Open `AiMenu.sln`, set `PD.UI` as the startup project, and run (F5 or Ctrl+F5). Swagger is available in Development.

Using dotnet CLI:
- Restore, build and run:
  - dotnet restore
  - dotnet build
  - cd PD.UI
  - dotnet run

By default Kestrel will choose a port; check console output for the exact URL (or set ASPNETCORE_URLS to a specific address).

Swagger UI (in development): `https://{host}:{port}/swagger`

---

## API reference (important endpoints)

General: Controllers return `ResultViewModel<T>` with status codes and messages. The DTOs are located under `Dtos/*`.

Note: Login returns a JWT token (string). Currently controllers are not decorated with [Authorize] in the shipped code, but the token generation is implemented in `PD.BL/Services/AuthService`.

All routes are defined explicitly in controllers; examples below.

### Authentication / Users

- POST /api/users/createUser
  - Body: RegisterDto
  - RegisterDto:
    ```json
    {
      "name": "Alice",
      "password": "P@ssw0rd",
      "email": "alice@example.com"
    }
    ```
  - Success: 201 Created (ResultViewModel<UserDto>)

- POST /api/users/login
  - Body: LoginDto (check `Dtos/UserDtos/LoginDto.cs`)
    ```json
    {
      "email": "alice@example.com",
      "password": "P@ssw0rd"
    }
    ```
  - Success: 200 OK with { "token": "<jwt-token>" } or null on failure.

- GET /api/users/{id}
  - Returns user by id.

- GET /api/users/getAllUsers
  - Returns list of users.

- PUT /api/users/updateUser/{id}
  - Body: UpdateUserDto

- PUT /api/users/updatePassword/{id}
  - Body: UpdatePasswordDto
    ```json
    {
      "oldPassword": "old",
      "newPassword": "new"
    }
    ```

- DELETE /api/users/deleteUser/{id}

### Menu items

- POST /api/menuItems/addMenuItem
  - Body: AddMenuItemDto (see DTOs under Dtos/MenuItemDto)
  - Returns created menu item (201 on success).

- GET /api/menuItems/getAllMenuItems
  - Returns list of menu items.

- GET /api/menuItems/getMenuItemById/{id}

- PUT /api/menuItems/updateMenuItem/{id}
  - Body: UpdateMenuItemDto

- DELETE /api/menuItems/deleteMenuItem/{id}

- GET /api/menuItems/getMenuItemsByCategory/{category}

- GET /api/menuItems/searchMenuItems?searchTerm=term

- GET /api/menuItems/getMenuItemsByPriceRange?minPrice=1&maxPrice=10

- GET /api/menuItems/getMenuItemByIngeredients?ingredients=Tomato

MenuItemDto (response):
```json
{
  "id": 1,
  "name": "Pizza Margherita",
  "price": 12.5,
  "description": "Classic pizza",
  "category": "Pizza",
  "ingeredents": "Tomato, Cheese, Basil",
  "imageUrl": "http://..."
}
```

### Order items

- POST /api/orderItems/addOrderItem?orderId={orderId}
  - Body: CreateOrderItemDto (see `Dtos/OrderItemDto/CreateOrderItemDto.cs`)
  - Creates order item; if orderId is null a new order may be created (check BL for behaviour).

- DELETE /api/orderItems/deleteOrderItem/{orderItemId}

- GET /api/orderItems/getOrderItemsByOrderId/{orderId}

- PUT /api/orderItems/setQuantity
  - Body: SetQuantityDto
    ```json
    {
      "orderItemId": 12,
      "quantity": 3
    }
    ```

---

## Testing the API

- PD.UI/PD.UI.http contains an example HTTP request that can be used with tools like VS Code REST Client or JetBrains HTTP Client.
- You can also use curl. Example: register, login and fetch users.

Register:
```bash
curl -X POST "http://localhost:5107/api/users/createUser" \
  -H "Content-Type: application/json" \
  -d '{"name":"Alice","email":"alice@example.com","password":"P@ssw0rd"}'
```

Login:
```bash
curl -X POST "http://localhost:5107/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"alice@example.com","password":"P@ssw0rd"}'
# Response: { "token": "<jwt>" }
```

Get all menu items:
```bash
curl "http://localhost:5107/api/menuItems/getAllMenuItems"
```

If Swagger is enabled (Development), visit:
- https://localhost:{port}/swagger

---

## Development notes

- Business logic:
  - `PD.BL/Services/*` contains services (UserService, MenuItemService, OrderItemService).
- Data access:
  - `PD.DAL/Context.cs` and `PD.DAL/Entitites/*` define EF models.
  - Repositories are under `PD.DAL` (BaseRepository, UserRepository).
- DTOs and validation:
  - DTOs: `Dtos/*`
  - Validators are in `PD.BL/Validators/*` (FluentValidation).
- AutoMapper mappings are in `PD.BL/Mappings/*`.

To modify validation rules, edit the appropriate validator under `PD.BL/Validators`. To change mapping, update `PD.BL/Mappings/*` profiles.

---

## Troubleshooting

- Database connection errors:
  - Verify `DefaultConnection` in `PD.UI/appsettings.json`.
  - Confirm SQL Server is running and the user has permission to create the database.
- Migrations/EF errors:
  - Ensure `dotnet-ef` is installed and the correct startup project is provided when executing `dotnet ef` commands.
- JWT / Authentication:
  - If tokens are not accepted in a future modification, check middleware and `[Authorize]` attributes (currently routes are public in the repository).

---

## Next steps & contribution

- Add automated tests (unit/integration).
- Add authentication middleware and protect routes with `[Authorize]`.
- Add seeding for initial data (roles, sample menu items).
- Improve error handling and logging.

If you want me to:
- Add a sample Dockerfile and docker-compose to run the API with SQL Server container.
- Add a `Makefile` or scripts for common tasks (migrate, run).
- Add CI pipeline for build and tests.

Please open an issue or edit the README and propose changes.

---
