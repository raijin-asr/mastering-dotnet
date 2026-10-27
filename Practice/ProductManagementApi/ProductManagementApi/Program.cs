using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container used without the MVC view-rendering features.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//Register Swagger services
builder.Services.AddEndpointsApiExplorer(); //Makes endpoint metadata available to API description tooling.
builder.Services.AddSwaggerGen(); //Generates an OpenAPI description of your API.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //Redirects eligible HTTP requests to HTTPS

app.UseAuthorization(); //Adds authorization middleware to the request pipeline. Authorization determines whether a user is allowed to access a protected resource.

app.MapControllers(); //It maps the routes defined by your controller attributes to endpoints that ASP.NET Core can execute.

app.Run(); //Starts the application. The server begins listening on the configured addresses and processing requests.
