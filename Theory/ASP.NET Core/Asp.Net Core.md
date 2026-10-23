### 1. ASP.NET Core Overview
- New Web framework of Microsoft redesigned to be fast, flexible and work across different platforms.
- Its smaller and more modular re-written framework of ASP.NET 4.6 but ASP.NET Core 1.0 is new not continuation of ASP.NET 4.6.
- Its open sourced and cloud optimized that helps to develop modern web applications to run on windows, Linux and mac.
- Includes MVC framework, combines with Web API into a single web programming framework.
- Its no longer based on System.Web.dll but on set of granular and well factored NuGet packages.
- Its cloud ready environment based configuration and has built in dependency injection DI support.
- It has Tag Helpers to make Razor markup more natural with HTML and ability to host on IIS or self host in own process.

### 2. The Evolution: ASP.NET 4.x vs. Modern .NET
- A. [ ASP.NET 4.x / .NET Framework ]
- Monolithic, Windows-only, IIS-bound
- Coupled to System.Web.dll (Heavy pipeline)
- Separate MVC vs. WebAPI controllers
- 
- B. [ .NET Core 1.0 - 2.0 (2016–2017) ] ── (Experimental Era)
- Rebuilt ground-up for cross-platform (Linux/macOS)
- Tried project.json -> Abandoned; restored .csproj

- ▼
- C. [ Modern .NET (8 / 9+) ]
- High performance, lightweight Kestrel server
- Built-in Dependency Injection (DI) & Middleware pipeline
- Unified Controllers & Minimal APIs

### 3. Why ASP.NET Core was Rebuilt Ground-Up
- a. Decoupling from System.Web.dll
- Old Way: In ASP.NET 4.x, every request passed through System.Web.dll, carrying decades of legacy weight and forcing a hard dependency on Windows IIS.
- New Way: Modern .NET uses a modular Middleware Pipeline (Program.cs). Your app only processes the code you explicitly include (UseRouting(), UseAuthentication(), etc.).
___
- b. Cross-Platform & Kestrel Web Server
- Old Way: IIS was the only primary host on Windows Server.   
- New Way: ASP.NET Core introduced Kestrel, an extremely lightweight cross-platform web server. You can run your apps inside Docker containers, Linux, or behind Nginx/IIS as a reverse proxy.
___
c.  Unified MVC and Web API
- Old Way: In .NET Framework, System.Web.Mvc.Controller and System.Web.Http.ApiController were completely separate classes with different routing and dependency models.
- New Way: There is only one base ControllerBase class. The pipeline is identical whether you are returning HTML views or JSON payloads.
___
d. Built-in Dependency Injection & Configuration
- Old Way: You had to configure heavy web.config XML files and bring in third-party containers (Autofac, Ninject) for Dependency Injection.   
- New Way: JSON-based configuration (appsettings.json) and native, high-performance DI (AddScoped, AddSingleton) are built right into the core framework.
___


### 3. Middleware
- In ASP.NET Core, middleware are pieces of software (components) assembled into an application pipeline to handle requests and responses.
- Every request passes through this pipeline, where each middleware can inspect, modify, or terminate the request (and then pass it on to the next middleware using next()).

<img width="600" height="182" alt="image" src="https://github.com/user-attachments/assets/0c979ab0-45ab-4502-becd-71e7b0585d64" />


- Logger sees everything about the incoming request, and records some information and then pass along this request to the next piece of middleware.
- An authorizer looks for specific cookie or access tokens in the HTTP headers.
- If the authorizer finds a token, it allows the request to proceed else will respond to the request with an HTTP error code or redirect code to send the user to a login page..
- The router looks over the application for URL or something to respond to and if the router doesn't find anything to respond to, the router itself might return a 404 Not Found error.
```
//example from OLD ASP.NET Core
using Microsoft.AspNet.Builder; 
using Microsoft.AspNet.Hosting; 
using Microsoft.AspNet.Http; 

using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Configuration;  

namespace FirstAppDemo { 
   public class Startup { 
      public Startup() { 
         var builder = new ConfigurationBuilder() 
            .AddJsonFile("AppSettings.json"); 
         Configuration = builder.Build(); 
      }  
      public IConfiguration Configuration { get; set; }  
       
      // This method gets called by the runtime. 
      // Use this method to add services to the container. 
      // For more information on how to configure your application, 
      public void ConfigureServices(IServiceCollection services) { 
      }  
      
      // This method gets called by the runtime.  
      // Use this method to configure the HTTP request pipeline. 
      public void Configure(IApplicationBuilder app) { 
         app.UseIISPlatformHandler();   //to work with Windows authentication.
  	app.UseWelcomePage();   //shows welcome page
	//or use this app.UseRuntimeInfoPage();  //also show root page
	app.UseDeveloperExceptionPage();   //for exceptions
	app.UseStaticFiles();  //to show content of static files such as html
         //or use app.UseDefaultFiles();  to show index.html file by default
         app.Run(async (context) => { 
            var msg = Configuration["message"]; 
            await context.Response.WriteAsync(msg); 
         });  
      }  
      // Entry point for the application. 
      public static void Main(string[] args) => WebApplication.Run<Startup>(args); 
   } 
} 
```

### 4. MVC
- We can technically build an entire application using only middleware, but ASP.NET Core MVC gives us the features that we can use to easily create HTML pages and HTTP-based APIs.
- Building an application using only middleware means writing all your logic directly into these request-handling functions (using app.Run(), app.Use(), etc.) without using higher-level abstractions like Controllers, Views, or Routing frameworks.
- The Hard Way (Only Middleware): You would manually inspect the incoming HTTP request URL (e.g., if path is /about, write out HTML manually), read request bodies yourself, and handle routing entirely by writing custom if/else statements for every single endpoint
- The Framework Way (MVC/Minimal APIs): ASP.NET Core MVC provides a structured architecture out-of-the-box that handles routing, model binding, validation, serialization, and HTML generation (via Razor views) so you don't have to write low-level plumbing.
- Classic ASP.NET (The Old .NET / .NET Framework): ASP.NET MVC was introduced as a separate, major framework framework (like ASP.NET MVC 4 or 5) running on the heavy, Windows-only .NET Framework.
- ASP.NET Core (The Modern .NET): ASP.NET Core unified everything. MVC is no longer a separate framework; it is built right into ASP.NET Core as a set of services and libraries that you can add to your web application (builder.Services.AddControllersWithViews()).
- In modern .NET (Core / .NET 6/7/8/9+), MVC sits on top of the ASP.NET Core foundational request pipeline, giving you the familiar Controller-View-Model structure
___
- Approach 1: The Hard Way (Using Pure Middleware)
- In this approach, you write everything directly inside Program.cs. 
- You have to manually check paths, manage headers, and write out raw HTML strings. 
- If your application grows to hundreds of pages, this file quickly becomes an unmaintainable mess of if/else statements.
```
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Custom middleware for handling the /about route
app.Use(async (context, next) =>
{
    if (context.Request.Path.Equals("/about", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync("<!DOCTYPE html>");
        await context.Response.WriteAsync("<html><body>");
        await context.Response.WriteAsync("<h1>About Us</h1>");
        await context.Response.WriteAsync("<p>This page was built using pure middleware.</p>");
        await context.Response.WriteAsync("</body></html>");
        return; // Short-circuit the pipeline so it doesn't call 'next()'
    }

    // If it's not /about, pass to the next middleware in the pipeline
    await next();
});

// Default middleware for everything else
app.Run(async context =>
{
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync("Hello from the root/fallback middleware!");
});

app.Run();
```

___
- Approach 2: The Framework Way (Using ASP.NET Core MVC)
- With MVC, ASP.NET Core automates the routing, gives you dedicated Controller classes to organize your business logic, and lets you use Razor views (.cshtml) for clean HTML separation.
- 1. Registering MVC in Program.cs
```
var builder = WebApplication.CreateBuilder(args);

// Add MVC services (Controllers and Views) to the dependency injection container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Map incoming URLs to Controllers and Actions (e.g., /Home/About)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

- 2. Creating the Controller (Controllers/HomeController.cs)
```
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    // Handles requests to /Home/About or /Home/About
    public IActionResult About()
    {
        // You can pass data from the backend to the view easily here
        ViewData["Title"] = "About Us";
        return View(); // Automatically looks for Views/Home/About.cshtml
    }
}
```

- 3. Creating the HTML View (Views/Home/About.cshtml)
```
@{
    ViewData["Title"] = "About Us";
}

<h1>@ViewData["Title"]</h1>
<p>This page was rendered cleanly using an MVC Controller and a Razor View template.</p>
```
___

- Middleware is great for cross-cutting tasks that apply to every request (like security headers, authentication, logging, or serving static images/CSS).
- MVC gives you structural organization (separation of concerns), built-in routing rules, model validation, layout templates, and HTML generation tools so you don't have to write raw HTML strings inside your C# code

### 4.1. MVC Design Pattern
- popular design pattern for the user interface layer of a software application.
- separates the user interface (UI) of an application into the following three parts: Model, View and Controller
- The Model − A set of classes that describes the data you are working with as well as the business logic.
- The View − Defines how the applications UI will be displayed. It is a pure HTML which decides how the UI is going to look like.
- The Controller − A set of classes that handles communication from the user, overall application flow, and application-specific logic.

<img width="398" height="226" alt="image" src="https://github.com/user-attachments/assets/b7d0e782-29f1-4e19-810f-617006d16710" />

- The view renders UI and talks to a model, and that model contains all the data that the view needs to display.
- The controller organizes everything. When an HTTP request arrives for an MVC application, the request gets routed to a controller, and then it's up to the controller to talk to either the database, the file system, or a model.


#### 4.2 Routing
- Routing is the mechanism that maps incoming HTTP requests to executable endpoints, such as controller actions. 
- ASP.NET Core supports two primary ways of routing: **Conventional Routing** and **Attribute Routing**.

___

- In older versions, routing configuration and MVC middleware were registered inside the `Configure` method of the `Startup` class using `app.UseMvc()` or `app.UseMvcWithDefaultRoute()`.
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Older approach using UseMvc
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
}
```
- UseMvc() Middleware: Handled both route matching and dispatching inside a single bulky middleware component.
- IRouteBuilder & MapRoute(): Used MapRoute with a template parameter.
- Implicit Conventions: Heavily relied on token parsing ({controller}, {action}, {id?}) where defaults fell back to Home and Index.

___

- Modern Version Style (.NET 6 / 7 / 8 / 9 / 10+) splits routing into a decoupled, high-performance architecture called Endpoint Routing, making use of top-level statements and minimal hosting models (Program.cs).
- Instead of UseMvc, modern apps use the separated middleware pipeline: UseRouting() and MapControllerRoute() (or MapDefaultControllerRoute()).
```
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting(); // Matches request to an endpoint

app.UseAuthorization();

// Maps conventional routes to controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```
- separation of concerns as UseRouting() Looks at incoming requests and matches them against defined endpoints and UseEndpoints() / MapControllerRoute() Executes the matched endpoint.
- pattern over template as the keyword parameter changed from template: to pattern:
- Better Performance as Endpoint routing decouples URL matching from execution, allowing other middleware (like CORS or Authorization) to run between matching and execution.

#### 4.2.1 Routing Types
___
- A. Conventional Routing
- Establishes a predictable convention for URL paths. Commonly used for traditional MVC web apps.
- Pattern structure: "{controller=Home}/{action=Index}/{id?}"
- Segment 1 ({controller}): Maps to the controller name (defaults to Home).
- Segment 2 ({action}): Maps to the action method inside that controller (defaults to Index).
- Segment 3 ({id?}): An optional parameter passed to the action.
___
- B. Attribute Routing
- Uses C# attributes directly on controllers and actions. Highly recommended for building RESTful APIs.
- Gives precise control over the exact URL structure, independent of controller/action naming conventions, and natively supports HTTP verb constraints ([HttpGet], [HttpPost], etc.).
```
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet("{id:int}")] // Matches GET: /api/products/5
    public IActionResult GetProduct(int id)
    {
        // Implementation
        return Ok();
    }
}
```
___


### 4.3 Action Results
- ActionResult class is the base for all the action results.
- Actions basically return different types of Action Results such as simple strings or integers or complex objects like students implementing IActionResult interface.
___
- List of action results and their behavior:
- ContentResult: Returns a string
- FileContentResult: Returns file content
- FilePathResult: Returns file content
- FileStreamResult: Returns file content.
- EmptyResult: Returns nothing
- JavaScriptResult: Returns script for execution
- JsonResult: Returns JSON formatted data
- RedirectToResult: Redirects to the specified URL
- HttpUnauthorizedResult: Returns 403 HTTP Status code
- RedirectToRouteResult: Redirect to different action/ different controller action
- ViewResult: Received as a response for view engine
- PartialViewResult: Received as a response for view engine
___
- simple example to return ContentResult
```
using Microsoft.AspNet.Mvc; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;  

namespace FirstAppdemo.Controllers  { 
   public class HomeController : Controller { 
      public ContentResult Index() { 
         return Content("Hello, World! this message is from 
            Home Controller using the Action Result"); 
      } 
   } 
}
```

- example to return an Employee object:
```
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;  

namespace FirstAppDemo.Models { 
   public class Employee { 
      public int ID { get; set; } 
      public string Name { get; set} 
   } 
}
```
- here returning ObjectResult which will be serialized into XML or JSON to represent it in HTTP response.

```
using FirstAppDemo.Models; 
using Microsoft.AspNet.Mvc; 

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;  

namespace FirstAppdemo.Controllers { 
   public class HomeController : Controller { 
      public ObjectResult Index() { 
         var employee = new Employee { ID = 1, Name = "Mark Upston"}; 
         return new ObjectResult(employee); 
      } 
   } 
} 
```
___
- Old Tradition Approach: ( IActionResult & ActionResult<T>)
- Used heavily in classic ASP.NET MVC and standard ASP.NET Core Controllers.
- It relies on heavy MVC infrastructure, filters, and model binding.
- An interface (IActionResult) or generic class (ActionResult<T>) returned by a controller method that represents various HTTP response types (like JSON, HTML, or status codes).
- Tied directly to the Controller architecture (System.Web.Mvc or Microsoft.AspNetCore.Mvc).
- Slightly slow coz Relies on heavy MVC pipeline and heavy overhead like controller context, model state validation, and filters.
-  Helper classes are Ok(), BadRequest(), NotFound().
- Used for Large, traditional enterprise MVC apps.
```
[HttpGet("{id}")]
public IActionResult GetProduct(int id)
{
    if (id == 0) return BadRequest("Invalid ID"); // Returns HTTP 400
    var product = new { Id = id, Name = "Laptop" };
    return Ok(product); // Returns HTTP 200 with JSON data
}
```
___
- New/Modern Approach (IResult & Minimal APIs): 
- Introduced in modern .NET (starting with .NET 6 onwards) alongside Minimal APIs to make applications faster, lighter, and completely controller-free.
- IResult is a modern interface representing the result of an HTTP endpoint. It is built to work seamlessly with Minimal APIs (though it can be used in controllers too).
- Blazing Fast: Eliminates the heavyweight MVC/Controller pipeline overhead.
- Uses static helper methods from the Results class (e.g., Results.Ok(), Results.NotFound(), Results.BadRequest())
- Ideal for lightweight microservices and fast endpoints.
```
// Inside Program.cs (Minimal API style)
app.MapGet("/products/{id}", IResult (int id) => 
{
    if (id == 0) return Results.BadRequest("Invalid ID");
    var product = new { Id = id, Name = "Laptop" };
    return Results.Ok(product);
});
```
___

#### 4.3.1. Controllers vs API
- Controllers are not just for old .NET. Modern .NET lets you choose between Controller-based APIs and Minimal APIs. 
- Both are modern; controllers just offer more structure for large applications.
- In ASP.NET Core, there are two different ways to build an API.
- 1. Controller-Based APIs (ControllerBase):
- You group endpoints inside a Controller class (e.g., ProductsController).
- Used in: Both old ASP.NET MVC and modern .NET Core. They are still heavily used today in almost all enterprise apps.

- 2. Minimal APIs (Results / IResult):
- You write endpoints directly in Program.cs without any controllers.
- Introduced in: Modern .NET (from .NET 6 onwards) for lightweight, high-performance microservices.


#### 4.3.2. Layered Architecture (or 3-Tier Architecture)
- Controllers/APIs -> Services -> Repositories Architecture is the standard way professional .NET developers organize code so it doesn't become a messy, unmaintainable single file.
- If your database changes or you want to switch from a React frontend to a Mobile App (Flutter/React Native), you don't have to rewrite your business logic or database code. You just build a new Controller/API layer that plugs right into your existing Services and Repositories!
__
- Think of it like a restaurant:
- The Waiter (Controllers / APIs): Takes the order from the customer (HTTP Request), passes it to the kitchen, and brings back the food (HTTP Response). They don't cook the food; they just handle the communication.
- The Chef (Services / Business Logic): Does the actual work. If a customer orders a discounted item, the Service layer calculates the discount, checks if the user is eligible, and applies business rules.
- The Pantry/Inventory (Repositories / Data Access): Goes to the database (via EF Core) to fetch raw ingredients (rows of data) or save new ones.
___
- Layer 1: Controllers / APIs (Presentation):
- Receives the HTTP request, validates basic input, calls the Service, and returns JSON.
```
public IActionResult Get(int id) => Ok(_productService.GetById(id));
```
- Layer 2: Services (Business Logic)
- Contains the brain of your application. 
- If you need to send an email when a product is created, or calculate total tax, it happens here. It doesn't know anything about HTTP or Controllers.
- Layer 3: Repositories (Data Access):
- Pure database operations. 
- It talks to Entity Framework Core to run queries like _context.Products.Find(id). ( In very modern apps, people sometimes skip the Repository layer and inject EF Core's DbContext directly into Services, but the concept remains the same).

### 4.4 Views
- In an ASP.NET Core MVC application, there is nothing like a page and view is same like page or file.
- With the MVC framework, the most popular method for creating HTML is to use the Razor view engine of ASP.NET MVC.
- A controller action produces a ViewResult object, and a ViewResult can carry the name of the Razor view that we want to use and the framework will find the view on the file system, execute the view, which produces HTML, and it is this HTML which the framework sends back to the client.
```
// MVC View Page called index.cshtml
<html xmlns = "http://www.w3.org/1999/xhtml"> 
   <head> 
      <title>Home</title> 
   </head> 
   
   <body> 
      <h1>Welcome!</h1> 
      
      <div> 
         @Model.Name 
      </div> 
   </body> 
</html> 
```
- HomeController and View method with new ViewResult as return type of Index method and some parameters such as employee model.
```
using FirstAppDemo.Models; 
using Microsoft.AspNet.Mvc; 

using System; 
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;  

namespace FirstAppdemo.Controllers { 
   public class HomeController : Controller { 
      public ViewResult Index() { 
         var employee = new Employee { ID = 1, Name = "Ameer"}; 
         return View(employee); 
      } 
   } 
} 
```

- View should only handle presentation UI of web application taking data and rendering as HTML and should never contain heavy business logic, database queries, or complex code.
- ASP.NET Core views use .cshtml files, which blend standard HTML with C# code using the @ symbol.
___
- 1. Old .Net(ASP.NET MVC / WebForms)
- Engine & Performance: Older Razor engines; heavier dependency footprint, slower startup and execution.
- Project Structure: Cluttered configs (Global.asax, web.config), rigid folder setups, and heavy XML-based configurations.
- Page-Focused Options: Relied heavily on MVC (where every page needs a Controller) or WebForms (page lifecycle baggage).
- Tag Helpers: Relied on older HtmlHelpers (e.g., Html.TextBoxFor()), which looked like messy C# syntax mixed with HTML.
___
- 2. Modern ASP.NET Core (.NET 8/9/10)
- Engine & Performance: Built on a high-performance, cross-platform, optimized Razor engine integrated natively into modern Kestrel.
- Project Structure: Clean, minimal structure (Program.cs file), convention-based, and lightweight.
- Page-Focused Options: Introduces Razor Pages as a modern, streamlined alternative alongside MVC.
- Tag Helpers: Uses modern Tag Helpers (e.g., asp-for=""), which look and feel like natural HTML attributes.
- The @model Directive: Declares the strongly-typed data structure that the view expects to receive from the backend.   
- Layouts (_Layout.cshtml): Acts as a master template (header, footer, navigation bar) to prevent code duplication across multiple pages.   
- Partial Views: Reusable chunks of UI (like a user card or product snippet) used to keep code clean and modular.   
- View Components: Modern "mini-controllers". Unlike simple partial views, View Components can run backend logic or database queries before rendering widget-like HTML (e.g., a shopping cart summary).   
___
- When building modern ASP.NET Core apps, you generally choose between two styles of views:
- 1. MVC Views (Traditional Controller-Based): (Request -> Controller -> View.)
- Best for: Large-scale applications, APIs, or teams heavily accustomed to separating logic strictly into Controllers, Models, and Views.
- 2. Razor Pages (Modern Page-Focused): Request goes straight to a page file (.cshtml + a .cshtml.cs code-behind file).
- Best for: Form-heavy, CRUD-style, or content-driven apps where you want all code related to a single page kept neatly together without building a separate Controller.
___
- Razor was created specifically as a view engine for ASP.NET MVC (to replace the older, clunky WebForms syntax like <%= Response.Write(...) %>).
- However, years later, Microsoft realized that developers building simple pages didn't always want the overhead of creating a separate Controller file just to show a view. That led to the creation of Razor Pages (introduced in .NET Core 2.0) as an alternative to MVC, utilizing the same Razor syntax.

### 5. Entity Framework
- an Object-Relational Mapper (ORM) for .NET developers.
- Traditionally, to talk to a database, you had to write raw SQL queries (SELECT, INSERT, UPDATE, DELETE) and map the result rows manually back into your code.
- But it solve this issue by letting you interact with your database using C# classes and objects instead of SQL. EF automatically translates your C# code into SQL behind the scenes.

#### 5.1. EF Core Concepts
- A. Entities (Models) are simply a C# class that maps to a table in your database. Each property in the class represents a column in that table
```
public class Product 
{
    public int Id { get; set; }        // Primary Key
    public string Name { get; set; }   // Column
    public decimal Price { get; set; } // Column
}
```
___
- B. DbContext (The Bridge): The DbContext is the most important class in EF Core. It acts as a bridge between your C# application and the database.
- It tracks changes made to your objects. 
- It manages database connections. 
- It uses DbSet<T> to represent database tables.
```
public class AppDbContext : DbContext 
{
    public DbSet<Product> Products { get; set; } // Represents the 'Products' table
}
```
___
 - C. LINQ (Language Integrated Query): Instead of writing SQL strings, you query your database using C# LINQ syntax. 
- EF converts these queries into SQL automatically. 
```
// Example: Find all products cheaper than $10
var cheapProducts = context.Products.Where(p => p.Price < 10).ToList();
```  

#### 5.2. EF Development Appraoches
- EF Core primarily supports two main approaches: Code First and Database first.
- Code First (Most Popular for Beginners): You write your C# classes and DbContext first, and EF generates the database schema for you.  
- Database First: You already have an existing database, and EF generates the C# classes for you automatically.

### 5.3 How to Handle Database Changes (Migrations)
- Before you can run EF terminal commands, you need to install the EF command-line tool globally (you only need to do this once on your machine): " dotnet tool install --global dotnet-ef"
- When using the Code First approach, whenever you change your C# classes (e.g., add a new property to Product), you need to update your database. EF handles this via Migrations: 
- dotnet ef migrations add InitialCreate  - Creates a migration file detailing what changes to make.   
- dotnet ef database update - Applies those changes to the actual SQL database.
- dotnet ef migrations remove - to undo last migration if not applied yet
- dotnet ef migrations script -o migration.sql - Generate a raw SQL script from your migrations (great for production deployment):

#### 5.4 EF CRUD operations
- Create (Insert):
```
var product = new Product { Name = "Laptop", Price = 100};
context.Products.Add(product);
context.SaveChanges(); //commits changes to database
```

- Read(Select): 
``` var products = context.Products.ToList(); //Gets all records```

- Update:
```
var products = context.Products.Find(1); //find product with ID 1
product.Price = 100;
context.SaveChanges();// EF auto detect changes and updates
```

- Delete:
```
var product = context.Products.Find(1);
context.Products.Remove(product);
context.SaveChanges();
```

#### 5.5 Pros and Cons of EF:
- Pros:
- High Productivity: Write less boilerplate data-access code.
- Switch from SQL Server to PostgreSQL or SQLite with minimal code changes.
- Type Safety: Catch errors at compile-time using C# / LINQ rather than runtime SQL errors.
___
- Cons:
- Performance Overhead: Raw SQL can sometimes be faster for ultra-complex queries.
- "Black Box" Effect: If you don't understand how it translates code to SQL, it can generate inefficient queries.

### 6. Razor 

#### 6.1. Razor Layout Views
- A Layout view is a special Razor view with a *.cshtml extension i.e. _Layout.cshtml
- Its like skeleton or master template of your website.
- It has shared common elements to avoid repeated use of them such as header, logo, menu or sidebar or footer.
```
<!DOCTYPE html> 
<html> 
   
   <head> 
      <meta name = "viewport" content = "width = device-width" /> 
      <title>@ViewBag.Title</title> 
   </head> 

   <body> 
      <div> 
        @DateTime.Now 
      </div> 
      
      <div> 
        @RenderBody() 
      </div> 
      
   </body> 
</html>
```
- here @renderbody expression render other view and RenderBody() is a placeholder method inside the layout file that says: "Inject the specific content of the current view right here.
- ViewBag is a dynamic container used to pass data from a Controller to a View (from the backend C# code to the frontend HTML).
```
//In controller C#
public IActionResult Index()
{
    ViewBag.Message = "Welcome to my website!";
    ViewBag.VisitorCount = 42;
    return View();
}

//in .cshtml view
<h2>@ViewBag.Message</h2>
<p>Total visitors: @ViewBag.VisitorCount</p>
```

#### 6.2. Razor View Start
- The _ViewStart.cshtml file is a special configuration file in ASP.NET Core MVC and Razor Pages.
- Think of _ViewStart.cshtml as a "run this code before you render any view" instruction file.
- The Problem It Solves: Imagine you have 30 different pages (Home, About, Contact, Products, etc.) in your web app. Without a _ViewStart file, you would have to manually type this line at the top of every single individual page:
```
@{
    Layout = "_Layout";
}
```
- The Solution: By putting that line inside _ViewStart.cshtml once, the Razor engine automatically executes it before rendering any content page. Every page inherits your layout template automatically.
- It is typically located right inside the root of the Views folder (next to your Shared folder):
```
Views/
├── _ViewStart.cshtml  <-- Automatically runs for views in this folder & subfolders
├── _ViewImports.cshtml
├── Shared/
│   └── _Layout.cshtml
└── Home/
    └── Index.cshtml
```

#### 6.3 Razor View Import
- This view is new special global configuration file in MVC, old XML files to configure certain aspect of using or imports are gone and code is used now.
- Instead of writing the same code at the top of every single .cshtml file, you write it once here, and it applies automatically to all views in that folder and its subfolders.
- Imagine you have 20 different view pages, and almost all of them need to use your models (e.g., @using MyApp.Models) and Tag Helpers. Without this file, you would have to type those lines at the top of all 20 files.
```
@using MyWebsite.Models
@using MyWebsite.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

- 3 main things found in this file, such as @using statements, @addTagHelper statements: To enable ASP.NET Core Tag Helpers (which turn standard HTML tags into powerful server-side tools, like <form asp-action="Index">) and @inject statements: To inject services directly into your views if needed.

#### 6.4 Razor Tag Helpers
- Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. Tag helpers are a new feature and similar to HTML helpers, which help us render HTML.
- They look like standard HTML attributes, making your HTML clean and easy to read for both designers and developers.
- There are many built-in Tag Helpers for common tasks, such as creating forms, links, loading assets etc. Tag Helpers are authored in C#, and they target HTML elements based on the element name, the attribute name, or the parent tag.
- For example, the built-in LabelTagHelper can target the HTML <label> element when the LabelTagHelper attributes are applied.
- They reduce the explicit transitions between HTML and C# in Razor views and need to install NuGet Library to use it.
- anybody can author a tag helper, write own tag helper.
- By default, they are not just rendered down to the client, so need to tell Razor view engine about it by registering it using asterirk comman (#,) and assembly name as "@addTagHelper "*, Microsoft.AspNet.Mvc.TagHelpers"
```
//Normal code
@model HomePageViewModel  
@{  
   ViewBag.Title = "Home"; 
} 
<h1>Welcome!</h1> 

<table> 
   @foreach (var employee in Model.Employees) { 
      <tr> 
         <td>
            @Html.ActionLink(employee.Id.ToString(), "Details", new { id = employee.Id })
         </td> 
         <td>@employee.Name</td> 
      </tr> 
   } 
</table>
```
___
```
// same code with tag helper 
@model HomePageViewModel  
@{  
   ViewBag.Title = "Home"; 
} 
<h1>Welcome!</h1> 

<table> 
   @foreach (var employee in Model.Employees) { 
      <tr> 
         <td> 
            <a asp-action="Details" asp-route-id="@employee.Id" >Details</a> 
         </td> 
         <td>@employee.Name</td> 
      </tr> 
   } 
</table> 
```

- The asp-action = "Details" is the name of the action that we want to get to and asp-route-Id is taghelper.

### 7. Identity Framework

#### 7.1 Overview
- Used to implement forms authentication like windows auth or third party auth like Google, Facebook, Microsoft, or GitHub etc.
- It allows to add feature where users can register and login or logout.
- So here User Entity, -UserStore<T> class and IdentityDB<t> classes are needed.
- User Entity class inherit from base class in Identity framework which gives user properties like username and email address, which is plugged into UserStore class to create users and validate user passwords talking to database and user class is also plugged to IdentiyDB class which does EF DBContext to do actual database work.
- SignInManager and Identity Middleware are two pieces of Identity Framework where SignInManager can sign in user and sign out by manageing cookie, and Identiy Middleware reads this cookie and identify user.
- It handles secure user data storage, password hashing, roles, claims, tokens, and multi-factor authentication (MFA) out of the box, saving you from writing custom authentication systems.
___
- A. Core Models (Entities)
- ASP.NET Core Identity relies on Entity Framework Core (EF Core) to map entities to relational database tables:
- IdentityUser: Represents an application user (stores UserName, Email, PasswordHash, PhoneNumber, lockout states, etc.). Can be subclassed to add custom fields (e.g., FirstName, DateOfBirth).   
- IdentityRole: Represents user roles (e.g., Admin, User) used for role-based authorization.   
- IdentityDbContext: The EF Core database context class that manages identity tables (like AspNetUsers, AspNetRoles, etc.).
___
- B. Core Services & Managers
- UserManager<TUser>: A high-level service API used to manage users (create, update, delete, change passwords, manage roles, and validate credentials).   
- SignInManager<TSignIn>: Handles sign-in operations (e.g., signing users in/out, checking password credentials via PasswordSignInAsync, and managing external logins/2FA).
- RoleManager<TRole>: Manages role creation, deletion, and permission assignments.   
___
- C. Authorization Mechanisms
- Role-Based Authorization: Restricts access based on assigned groups.Example: [Authorize(Roles = "Admin")]
- Claims-Based Authorization: Restricts access based on key-value pairs (Claim) associated with the user profile (e.g., Permission = "EditProducts"). Example: [Authorize(Policy = "RequireEmployeeClaim")]
___
D. Typical Setup & Implementation Workflow
- First, Configure Services: Register Identity services in Program.cs linked to an EF Core database context.
```
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```
- Second, Add Middleware: Ensure authentication and authorization middlewares are active in the request pipeline (app.UseAuthentication(); and app.UseAuthorization();).
- Third, Database Migration: Generate and apply migrations via EF Core CLI commands (dotnet ef migrations add and dotnet ef database update) to spin up the required Identity tables.
- Last, UI Integration: Scaffold pre-built Razor Pages or UI components for login/registration or configure Minimal API endpoints (e.g., using AddIdentityApiEndpoints()).
___


#### 7.2 Authorize Attribute
- ASP.NET Core's [Authorize] attribute is a built-in security feature used to restrict access to controllers and actions so that only authenticated (and optionally authorized) users can access them.
- Authentication vs. Authorization: Authentication verifies who you are (logging in), while authorization determines what you are allowed to do (permissions, roles). [Authorize] handles both by ensuring the user is logged in first, and optionally checking if they meet specific criteria.
- Default Behavior: By default, placing [Authorize] with no parameters simply requires the user to be authenticated (logged in), regardless of their role or claims.

__
- How to use it:
- 1. Protecting an Entire Controller -- Apply the attribute at the top of a controller class to secure every action method inside it:
```
[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index() => View();
}
```
- 2. Protecting Specific Actions -- Apply it to individual action methods while leaving other parts of the controller public:
```
public class HomeController : Controller
{
    public IActionResult Index() => View(); // Public

    [Authorize]
    public IActionResult Settings() => View(); // Requires login
}
```
- 3. Role-Based Authorization -- Restrict access to users belonging to specific roles using the Roles property:
```
[Authorize(Roles = "Admin")]
public IActionResult AdminPanel() => View();

// Multiple roles (user must be in Admin OR Manager)
[Authorize(Roles = "Admin, Manager")]
public IActionResult Reports() => View();
```
- 4. Policy-Based Authorization
For more complex rules (e.g., "Must be at least 18 years old" or "Must own the document"), use custom policies:
```
[Authorize(Policy = "AtLeast21")]
public IActionResult BuyAlcohol() => View();
```
___
- Bypassing Authorization : Use the [AllowAnonymous] attribute to override an [Authorize] rule set at the controller level, making a specific action public:
```
[Authorize]
public class ProductsController : Controller
{
    [AllowAnonymous] // Anyone can view the catalog
    public IActionResult Catalog() => View();

    public IActionResult Checkout() => View(); // Requires login
}
```

#### 7.3 Step-by-Step Implementation Guide (MVC / Controllers)
- conceptual implementation showing how to register, log in, and log out a user using UserManager and SignInManager.
___
- Step A: Configure Identity in Program.cs. First, wire up the Identity services and your database context so the app knows how to talk to SQL Server.
```
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Default Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

var builderApp = builder.Build();
// ... middleware configuration (app.UseAuthentication(); app.UseAuthorization(); etc.)
```
___
- Step B: User Registration. Registration means capturing input data (Email and Password), creating a new IdentityUser instance, and saving it via UserManager.
- The View Model: (its not model that maps database table, its only required property or columns of table)
```
public class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}
```
- The Controller Action:
```
[HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (ModelState.IsValid)
    {
        // Map data to IdentityUser
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };

        // Create user with hashed password
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Automatically sign them in after registration (optional)
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // If creation failed, add errors to view
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
    return View(model);
}
```
 - Step C: User Login: Login checks if the submitted password matches the stored hash and issues an encrypted authentication cookie
- The View Model:
```
public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
```
- The Controller Action:
```
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        // Sign-in manager checks password and creates cookie
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, 
            model.Password, 
            model.RememberMe, 
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    }
    return View(model);
}
```
___
- Step D: User Logout: Logging out clears the authentication cookie from the browser session.(Note: Always use an [HttpPost] for logout to prevent Cross-Site Request Forgery (CSRF) vulnerabilities).
```
[HttpPost]
public async Task<IActionResult> Logout()
{
    // Clears the authentication cookie
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index", "Home");
}
```
___
- Checking Authentication Status in Views (.cshtml): easily toggle what options the user sees depending on whether they are logged in using Razor syntax
```
@if (SignInManager.IsSignedIn(User))
{
    <!-- Shown if logged in -->
    <span>Hello, @User.Identity.Name!</span>
    <form asp-controller="Account" asp-action="Logout" method="post">
        <button type="submit" class="btn btn-link">Logout</button>
    </form>
}
else
{
    <!-- Shown if logged out -->
    <a asp-controller="Account" asp-action="Register">Register</a>
    <a asp-controller="Account" asp-action="Login">Login</a>
}
```





