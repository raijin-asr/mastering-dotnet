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
                │
                ▼
- B. [ .NET Core 1.0 - 2.0 (2016–2017) ] ── (Experimental Era)
- Rebuilt ground-up for cross-platform (Linux/macOS)
- Tried project.json -> Abandoned; restored .csproj
                │
                ▼
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
