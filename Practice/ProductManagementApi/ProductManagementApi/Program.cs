using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagementApi.Data;
using ProductManagementApi.Models;
using ProductManagementApi.Services;
using System.Text;

//Flow: 1. Register services -> 2. Build app -> 3. If Testing: create database tables -> 4. Create Admin and Customer roles
//-> 5. If Development: optionally assign Admin to configured user -> 6. Configure middleware -> 7. Run app

# region 1. Services
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

//Authorization Policy (here centrally defined without putting logic inside controller actions)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));  //here role defined with dedicated method provided by ASP.NET core but its also claim, so can use RequireClaim method too

    options.AddPolicy("VerifiedAdmin", policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("EmailVerified", "true"); //here RequiredClaim used for other attributes of claim
    });

    options.AddPolicy("CanManagePrices", policy => //it does not care whether user is admin or not
        policy.RequireClaim("Permission", "ManagePrices")); //here claim used
    
});

var app = builder.Build();

#region 2. Create Roles
//Add test-environment database initialization for Integration testing
//the test database must have its Identity tables ready before the role-seeding code tries to access them
// Create the database schema when running integration tests.
if (app.Environment.IsEnvironment("Testing"))
{
    using var testScope = app.Services.CreateScope();

    var context = testScope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await context.Database.EnsureCreatedAsync();
}

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

    //it runs in Development, finds existing user by email, checks if that user has Admin role or assign it if needed
    if (app.Environment.IsDevelopment())
    {
        var adminEmail = app.Configuration["SeedAdmin:Email"];

        if (!string.IsNullOrWhiteSpace(adminEmail))
        {
            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                throw new InvalidOperationException(
                    $"Admin user '{adminEmail}' was not found.");
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(
                    adminUser, "Admin");

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Admin role: {errors}");
                }
            }
        }
    }
}

#endregion

#endregion


# region 3.Configure the HTTP request pipeline.

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

//Making it public allows to test project sincecompiler auto generated Program class is not accessible to another project
public partial class Program { } //this for Integration testing