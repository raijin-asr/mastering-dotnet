# Product Management API

A RESTful Web API built with **ASP.NET Core 8**, **Entity Framework Core 8**, and **SQLite** for managing products. The API supports CRUD operations, search, sorting, pagination, partial updates, request validation, and structured error handling.

This project is part of my .NET learning journey, following my ASP.NET Core MVC practice project. It focuses on understanding RESTful API development, clean separation of responsibilities, database integration, authentication, and HTTP status codes.

## Tech Stack

- **C#**
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core 8**
- **SQLite**
- **ASP.NET Core Identity**
- **JWT Bearer Authentication** (implementation in progress)
- **Swagger / OpenAPI**
- **Dependency Injection**
- **Visual Studio**

## Features

### Product Management
- Retrieve all products
- Retrieve a product by ID
- Create a new product
- Update an existing product using PUT
- Partially update a product using PATCH
- Delete a product
- Search products by name
- Sort products by supported fields
- Pagination with configurable page size

### Architecture and Data Management
- Service layer using `IProductService` and `ProductService`
- Dependency injection for application services
- DTOs (Data Transfer Objects) for request and response models
- EF Core with SQLite database integration
- Database schema management using EF Core migrations
- Asynchronous database operations

### Validation and Error Handling
- Request validation using Data Annotations
- Automatic model validation through `[ApiController]`
- Business validation for duplicate product names
- Case-insensitive unique product names
- Global exception handling
- Consistent error responses using Problem Details
- Logging using `ILogger`

### Authentication
- ASP.NET Core Identity integration
- User registration
- Password hashing and verification through Identity
- JWT configuration and token generation
- JWT-based authentication for protected endpoints (next implementation step)

## Screenshots

### Product Endpoints

<img width="1170" height="986" alt="image" src="https://github.com/user-attachments/assets/c0c574a5-ce63-4f8a-aaff-3527607bf058" />

*Figure 1: Product API HTTP methods in Swagger.*

### Login Endpoints with JWT Token Generated
<img width="1022" height="922" alt="image" src="https://github.com/user-attachments/assets/02be43cd-f7bc-4584-8adb-0c19f1046967" />
*Figure 2: Login Endpoints with JWT Token Generated*

### Authorize attribute to access protected endpoints
<img width="1102" height="932" alt="image" src="https://github.com/user-attachments/assets/0b8bb905-5130-4dcd-81ea-4ca374ba115a" />
*Figure 3: Logged in using Authorize button to access protected endpoints.*

### Search, Sorting and Pagination

<img width="1167" height="977" alt="image" src="https://github.com/user-attachments/assets/605493a9-8eba-4aed-8f77-cd92e8d6aeae" />

*Figure 4: Search, sorting and pagination through GET query parameters.*

## Project Structure

```text
ProductManagementApi/
├── Controllers/
│   ├── AuthController.cs
│   └── ProductsController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── Auth/
│   │   ├── LoginDto.cs
│   │   └── RegisterDto.cs
│   └── Products/
│       ├── CreateProductDto.cs
│       ├── UpdateProductDto.cs
│       ├── PatchProductDto.cs
│       ├── ProductResponseDto.cs
│       └── ProductListResponseDto.cs
├── Exceptions/
│   └── DuplicateProductNameException.cs
├── Migrations/
├── Models/
│   ├── ApplicationUser.cs
│   └── Product.cs
├── Services/
│   ├── IProductService.cs
│   └── ProductService.cs
├── Properties/
├── appsettings.json
├── Program.cs
├── ProductManagementApi.csproj
└── README.md
```

*The structure may evolve as additional features are implemented.*

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio or Visual Studio Code
- EF Core command-line tools

### 1. Clone the repository

```bash
git clone <this-repository-url>
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

### 4. Configure JWT secrets

JWT signing keys should not be committed to source control.

Initialize .NET User Secrets:

```bash
dotnet user-secrets init
```

Set the JWT signing key using a securely generated Base64-encoded key:

```bash
dotnet user-secrets set "Jwt:Key" "<your-generated-secret-key>"
```

The non-sensitive JWT settings can be configured in `appsettings.json`:

```json
{
  "Jwt": {
    "Issuer": "ProductManagementApi",
    "Audience": "ProductManagementApiClient",
    "ExpiresInMinutes": 60
  }
}
```

Do not publish your actual signing key.

### 5. Apply database migrations

If the database has not been created yet, run:

```bash
dotnet ef database update
```

The migration files are included in the project to create the database schema, including the product and Identity tables.

If needed, install the EF Core command-line tool:

```bash
dotnet tool install --global dotnet-ef --version 8.0.8
```

### 6. Run the application

```bash
dotnet run
```

Open the Swagger URL shown in the terminal. In Development mode, Swagger provides an interactive interface for testing the API endpoints.

## API Endpoints

### Product Endpoints

The base route is `/api/products`.

| HTTP Method | Endpoint | Description | Success Response |
|---|---|---|---|
| GET | `/api/products` | Retrieve products with search, sorting and pagination | `200 OK` |
| GET | `/api/products/{id}` | Retrieve a product by ID | `200 OK` |
| POST | `/api/products` | Create a product | `201 Created` |
| PUT | `/api/products/{id}` | Update a product | `204 No Content` |
| PATCH | `/api/products/{id}` | Partially update a product | `204 No Content` |
| DELETE | `/api/products/{id}` | Delete a product | `204 No Content` |

The API also uses:
- `400 Bad Request` for invalid request data.
- `404 Not Found` when a requested product does not exist.
- `409 Conflict` when a product name already exists.
- `500 Internal Server Error` for unexpected server errors.

### Authentication Endpoints

The authentication base route is `/api/auth`.

| HTTP Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate a user and return a JWT (implementation in progress) |

## Example Requests

### Get all products

```http
GET /api/products?page=1&pageSize=5&sortBy=price&descending=false
```

Optional query parameters include:

- `search`: Filter products by name.
- `sortBy`: Select a supported sorting field.
- `descending`: Choose descending or ascending order.
- `page`: Page number.
- `pageSize`: Number of products per page.

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

PUT replaces the product's editable fields with the submitted values.

```http
PUT /api/products/1
Content-Type: application/json
```

Request body:

```json
{
  "productName": "Updated Mobile",
  "price": 250,
  "stockLevel": 20
}
```

A successful update returns `204 No Content`.

### Partially update a product

PATCH updates only the fields provided in the request.

```http
PATCH /api/products/1
Content-Type: application/json
```

Request body:

```json
{
  "price": 275
}
```

A successful partial update returns `204 No Content`.

### Delete a product

```http
DELETE /api/products/1
```

A successful deletion returns `204 No Content`.

### Register a user

```http
POST /api/auth/register
Content-Type: application/json
```

Request body:

```json
{
  "email": "test@example.com",
  "password": "MyPassword123"
}
```

A successful registration returns `201 Created`.

## Database

The project uses **Entity Framework Core with SQLite**.

- `Product.cs` defines the product entity.
- `ApplicationUser.cs` extends ASP.NET Core Identity's user model.
- `AppDbContext.cs` derives from `IdentityDbContext<ApplicationUser>`.
- The `Products` table stores product information.
- Identity manages user-related tables, including `AspNetUsers`.
- A unique database index enforces case-insensitive product name uniqueness.
- EF Core migrations track and apply database schema changes.
- `SaveChangesAsync()` persists create, update, and delete operations.

The SQLite database file is generated locally and should generally not be committed to Git. Migration files should be committed so the database schema can be recreated.

## Learning Objectives

This project helps me practice:

- Understanding RESTful API design and HTTP methods
- Working with HTTP status codes
- Building controller-based Web APIs
- Separating business logic from controllers using a service layer
- Understanding dependency injection
- Using DTOs to define API request and response contracts
- Integrating EF Core with SQLite
- Implementing asynchronous database operations
- Managing database migrations
- Implementing validation and exception handling
- Understanding authentication and authorization with ASP.NET Core Identity and JWT
- Testing APIs using Swagger
- Complete JWT login and token generation
- Protect endpoints using `[Authorize]`
- Implement role-based authorization and claims
  
## Future Improvements

- Add unit tests using xUnit and Moq
- Add API integration tests
- Improve logging and production configuration
- Explore API versioning and EF Core performance improvements

## Author

Developed as part of my ongoing journey to learn and master the .NET ecosystem.
