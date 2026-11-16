using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagementApi.Data;
using ProductManagementApi.Models;
using ProductManagementApi.Services;
using System.Text;

# region Services
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

    // Define JWT Bearer authentication to tell Swagger API supports Bearer JWT authentication
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    // Apply Bearer authentication requirement (tells Swagger to apply security scheme to API operations)
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//register Identity Services
builder.Services
    .AddIdentityCore<ApplicationUser>(options => //Core user mgmt service
    {
        options.User.RequireUniqueEmail = true; //prevents multiple acconts using same email
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>() //tells identity to use exisiting EF core database context
    .AddDefaultTokenProviders(); //registers token providers for operation such as password reset and email confirmation.

// Register Data Protection services
builder.Services.AddDataProtection();


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

//JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

var app = builder.Build();

//Create roles automatically when app starts
using (var scope = app.Services.CreateScope()) //CreateScope creates DI scope so we can resolve scoped services safely during application startup.
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>(); //retreives Identity's role management service

    string[] roles = { "Admin", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create role {role}: {errors}");
            }
        }
    }
}

#endregion


# region Configure the HTTP request pipeline.

//Register middlware to catch unhandled exceptions and generates an HTTP 500 response using Problem Details when possible.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //Redirects eligible HTTP requests to HTTPS

app.UseAuthentication(); // this first before authorization becuase it reads the bearer token and tries to identify the user.
app.UseAuthorization(); //Adds authorization middleware to the request pipeline. Authorization determines whether a user is allowed to access a protected resource.

app.MapControllers(); //It maps the routes defined by your controller attributes to endpoints that ASP.NET Core can execute.

app.Run(); //Starts the application. The server begins listening on the configured addresses and processing requests.

#endregion