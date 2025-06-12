# Dotnet Store Backend

A RESTful API backend for an online store, built with ASP.NET Core and Entity Framework Core.

## Features

- User registration and login with JWT authentication
- Product and category management (CRUD)
- Order management (CRUD)
- In-memory shopping cart service
- Entity Framework Core with SQL Server
- OpenAPI/Swagger documentation

## Project Structure

```
dotnet-store-backend/
├── Controllers/         # API controllers (Auth, Products, Categories, Orders, Weather)
├── Data/                # Entity Framework DbContext
├── DTOs/                # Data Transfer Objects
├── Migrations/          # EF Core migrations
├── Models/              # Entity models
├── Services/            # Business logic services
├── appsettings.json     # Main configuration
├── Program.cs           # Application entry point
└── ...
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the repository**
   ```sh
   git clone https://github.com/niranjanimalka98/dotnet-store-backend.git
   cd dotnet-store-backend
   ```

2. **Configure the database**

   Update the `ConnectionStrings:DefaultConnection` in [`appsettings.json`](dotnet-store-backend/appsettings.json) with your SQL Server credentials.

3. **Apply migrations**
   ```sh
   dotnet ef database update --project dotnet-store-backend/dotnet-store-backend.csproj
   ```

4. **Run the application**
   ```sh
   dotnet run --project dotnet-store-backend/dotnet-store-backend.csproj
   ```

   The API will be available at `http://localhost:5124`.

### API Endpoints

- **Authentication:**  
  - `POST /api/auth/register` — Register a new user
  - `POST /api/auth/login` — Login and receive JWT token

- **Products:**  
  - `GET /api/products` — List products
  - `POST /api/products` — Create product
  - `PUT /api/products/{id}` — Update product
  - `DELETE /api/products/{id}` — Delete product

- **Categories:**  
  - `GET /api/categories` — List categories
  - `POST /api/categories` — Create category
  - `PUT /api/categories/{id}` — Update category
  - `DELETE /api/categories/{id}` — Delete category

- **Orders:**  
  - `GET /api/orders` — List orders
  - `POST /api/orders` — Create order
  - `PUT /api/orders/{id}` — Update order
  - `DELETE /api/orders/{id}` — Delete order

### Configuration

- **JWT Secret:**  
  Set `Jwt:Key` in [`appsettings.json`](dotnet-store-backend/appsettings.json) for token signing.
- **Database:**  
  Set `ConnectionStrings:DefaultConnection` for your SQL Server instance.

### Development

- OpenAPI/Swagger documentation is available at `/swagger` when running in development mode.
- Modify controllers and models in the respective folders to extend functionality.

## License

MIT License

---
