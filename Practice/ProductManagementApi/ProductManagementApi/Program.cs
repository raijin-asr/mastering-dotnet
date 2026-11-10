using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container used without the MVC view-rendering features.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//Register Swagger services
builder.Services.AddEndpointsApiExplorer(); //Makes endpoint metadata available to API description tooling.
builder.Services.AddSwaggerGen(options => //Generates an OpenAPI description of API.
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product Management API",
        Version = "v1",
        Description = "A RESTful API for managing products. Developed by Ameer (aka Raijin)."
    });
});

//standard format for communicating API errors.
//Register built-in exception handling Service
builder.Services.AddProblemDetails();

//Configure automatic validation responses
builder.Services.Configure<ApiBehaviorOptions>(options => // ApiBehavior Controls automatic behaviors provided by [ApiController]
{
    options.InvalidModelStateResponseFactory = context =>  //InvalidModelStateResponseFactory Allows to customize what ASP.NET Core returns when model validation fails.
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred."
        };

        return new BadRequestObjectResult(problemDetails);
    };
});

//Register service with DI
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//Register middlware to catch unhandled exceptions and generates an HTTP 500 response using Problem Details when possible.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //Redirects eligible HTTP requests to HTTPS

app.UseAuthorization(); //Adds authorization middleware to the request pipeline. Authorization determines whether a user is allowed to access a protected resource.

app.MapControllers(); //It maps the routes defined by your controller attributes to endpoints that ASP.NET Core can execute.

app.Run(); //Starts the application. The server begins listening on the configured addresses and processing requests.
