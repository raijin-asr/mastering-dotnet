# ASP.NET Core MVC — Product Management

A hands-on ASP.NET Core MVC project built to practise CRUD operations, Entity Framework Core, SQLite, Razor views, search, sorting, and pagination.

## Overview

This application provides a simple interface for managing products and inventory. It follows the Model-View-Controller (MVC) pattern and uses Entity Framework Core with SQLite for data persistence.

## Features

- **Product CRUD:** Add, view, edit, and delete products.
- **Product listing:** Display product IDs, names, prices, and stock levels.
- **Search:** Filter products by name.
- **Sorting:** Sort products by name or price in ascending or descending order.
- **Pagination:** Display products in pages to make larger lists easier to navigate.
- **ViewModels:** Separate form and list-page data from the database entity.
- **Razor views:** Build server-rendered HTML pages using ASP.NET Core MVC.
- **EF Core migrations:** Manage database schema changes through migrations.
- **SQLite persistence:** Store product data in a local database.

## Tech Stack

- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core 8
- SQLite
- Razor Views
- Bootstrap
- Git and GitHub

## Screenshots

  <img width="1762" height="1001" alt="image" src="https://github.com/user-attachments/assets/8311fe43-d5a9-4620-9ae6-af95711d4b5d" />
- Fig 1: Home Page showing all Products (with Search, Sorting and Pagination)
___
<img width="1467" height="971" alt="image" src="https://github.com/user-attachments/assets/bdef8dfc-3e2d-4ed9-909e-561f77d1e520" />

- Fig 2: Add Product Page

<img width="1226" height="977" alt="image" src="https://github.com/user-attachments/assets/2e3c3f81-0bf3-48d8-8693-f6fdad031612" />

- Fig 3: Edit Product Page

<img width="1362" height="982" alt="image" src="https://github.com/user-attachments/assets/8b8a5cdf-177c-4116-8ce7-cbfe26b3c4bc" />

- Fig 4: Delete Product Page

## Project Structure

- `Controllers/` — Handles incoming requests and application flow.
- `Models/` — Contains the product entity and ViewModels.
- `Data/` — Contains the Entity Framework Core `AppDbContext`.
- `Views/` — Contains Razor pages for product management.
- `Migrations/` — Contains EF Core database migration files.
- `wwwroot/` — Contains static assets such as CSS and JavaScript.

## Getting Started

### Prerequisites

- .NET 8 SDK
- An IDE such as Visual Studio or VS Code

### Run the application

1. Clone this repository.
2. Open a terminal in the directory containing `PracticeSandBox.csproj`.
3. Restore dependencies:

   ```bash
   dotnet restore
   ```

4. Apply the database migrations:

   ```bash
   dotnet ef database update
   ```

   If the EF Core CLI tool is not installed, install it before running the command.

5. Start the application:

   ```bash
   dotnet run
   ```

6. Open the local URL shown in the terminal.

The SQLite database is created locally when the migrations are applied. The database file is not included in the repository.

## What I Practised

- Understanding the MVC request lifecycle.
- Routing requests to controller actions.
- Passing strongly typed ViewModels from controllers to Razor views.
- Querying and updating data with Entity Framework Core.
- Implementing GET and POST actions for CRUD operations.
- Combining search, sorting, and pagination in a database query.
- Applying EF Core migrations and managing source control with Git.

## Learning Journey

This project is part of my ongoing ASP.NET Core learning journey. The original learning repository contains the broader practice history and theory notes.

## Future Improvements

- Strengthen validation and error handling.
- Improve automated testing.
- Add authentication and authorization.
- Explore an ASP.NET Core Web API implementation of the same product-management domain.
