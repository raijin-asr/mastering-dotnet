# Product Management API

A RESTful Web API built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQLite** to perform CRUD (Create, Read, Update, Delete) operations on products.

This project is part of my .NET learning journey, following my ASP.NET Core MVC practice project. The goal is to understand Web API development, RESTful endpoints, database integration, and HTTP status codes.

## Tech Stack

- **C#**
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core 8**
- **SQLite**
- **Swagger / OpenAPI**
- **Visual Studio**

## Features

- Retrieve all products
- Retrieve a product by ID
- Create a new product
- Update an existing product
- Delete a product
- Database integration using EF Core and SQLite
- Database schema management using EF Core migrations
- Interactive API testing with Swagger
- Asynchronous database operations

## Project Structure

```text
ProductManagementApi/
├── Controllers/
│   └── ProductsController.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
├── Models/
│   └── Product.cs
├── Properties/
├── appsettings.json
├── Program.cs
├── ProductManagementApi.csproj
└── README.md
```

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio or Visual Studio Code
- EF Core command-line tools

### 1. Clone the repository

```bash
git clone <your-repository-url>
```

Navigate to the folder containing `ProductManagementApi.csproj`:

```bash
cd ProductManagementApi/ProductManagementApi
```

Adjust the path if your repository has a different folder structure.

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the database

The project uses SQLite. The connection string in `appsettings.json` is:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=products.db"
  }
}
```

### 4. Apply database migrations

If the database has not been created yet, run:

```bash
dotnet ef database update
```

The migration files are included in the project to create the database schema.

If needed, install the EF Core command-line tool:

```bash
dotnet tool install --global dotnet-ef --version 8.0.8
```

### 5. Run the application

```bash
dotnet run
```

Open the Swagger URL shown in the terminal. In Development mode, Swagger provides an interactive interface for testing the API endpoints.

## API Endpoints

The base route is `/api/products`.

| HTTP Method | Endpoint | Description | Success Response |
|---|---|---|---|
| GET | `/api/products` | Retrieve all products | `200 OK` |
| GET | `/api/products/{id}` | Retrieve a product by ID | `200 OK` |
| POST | `/api/products` | Create a product | `201 Created` |
| PUT | `/api/products/{id}` | Update a product | `204 No Content` |
| DELETE | `/api/products/{id}` | Delete a product | `204 No Content` |

The API returns `404 Not Found` when a requested product does not exist.

## Example Requests

### Get all products

```http
GET /api/products
```

Example response:

```json
[
  {
    "id": 1,
    "productName": "Mobile",
    "price": 19,
    "stockLevel": 10
  },
  {
    "id": 2,
    "productName": "Charger",
    "price": 5,
    "stockLevel": 20
  }
]
```

### Create a product

```http
POST /api/products
Content-Type: application/json
```

Request body:

```json
{
  "productName": "Keyboard",
  "price": 25,
  "stockLevel": 15
}
```

A successful request returns `201 Created` with the created product and a URL for retrieving it.

### Update a product

```http
PUT /api/products/1
Content-Type: application/json
```

Request body:

```json
{
  "id": 1,
  "productName": "Updated Mobile",
  "price": 250,
  "stockLevel": 20
}
```

A successful update returns `204 No Content`.

### Delete a product

```http
DELETE /api/products/1
```

A successful deletion returns `204 No Content`.

## Database

The project uses **Entity Framework Core with SQLite**.

- `Product.cs` defines the product entity.
- `AppDbContext.cs` manages database access.
- The `Products` table stores product information.
- EF Core migrations track and apply database schema changes.
- `SaveChangesAsync()` persists create, update, and delete operations.

The SQLite database file is generated locally and should generally not be committed to Git. The migration files should be committed so the database schema can be recreated.

## Learning Objectives

This project helps me practice:

- Understanding RESTful API design
- Working with HTTP methods and status codes
- Creating controller-based API endpoints
- Using dependency injection
- Integrating EF Core with SQLite
- Implementing asynchronous database operations
- Managing database migrations
- Testing APIs using Swagger

## Future Improvements

- Implement DTOs (Data Transfer Objects)
- Add request validation and improved error handling
- Improve API response consistency
- Add filtering, searching, sorting, and pagination
- Implement authentication and authorization
- Add unit and integration tests
- Explore API versioning and production configuration

## Author

Developed as part of my ongoing journey to learn and master the .NET ecosystem.
