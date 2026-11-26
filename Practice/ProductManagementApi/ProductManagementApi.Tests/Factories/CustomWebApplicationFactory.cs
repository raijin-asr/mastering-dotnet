using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductManagementApi.Data;

namespace ProductManagementApi.Tests.Factories
{
    //1. Create factory class
    public class CustomWebApplicationFactory  //CustomWebApplicationFactory ets us customize that application specifically for testing.
        : WebApplicationFactory<Program> //this tells ASP.NET core to host Program of API
    {
        //2. Add a SQLite in-memory connection
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactory() //constructor having connection open for lifetime of the factory
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open(); //open connection bcoz SQLite memory database only exists its open
        }

        //3. method to override application's database registration
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //Use test environment by factory
            builder.UseEnvironment("Testing");

            //Add JWT config even if its test for separate test key, issuer and audience
            // Supply JWT settings for the test environment.
            builder.UseSetting(  //direct approach to supply JWT setting as following configureAppConfig gave error
                "Jwt:Key",
                "integration-test-signing-key-12345678901234567890");

            builder.UseSetting(
                "Jwt:Issuer",
                "ProductManagementApi.Tests");

            builder.UseSetting(
                "Jwt:Audience",
                "ProductManagementApi.Tests");

            //builder.ConfigureAppConfiguration((context, config) =>
            //{
            //    config.AddInMemoryCollection(new Dictionary<string, string?>
            //    {
            //        ["Jwt:Key"] = "integration-test-signing-key-12345678901234567890",
            //        ["Jwt:Issuer"] = "ProductManagementApi.Tests",
            //        ["Jwt:Audience"] = "ProductManagementApi.Tests"
            //    });
            //});

            //replacing dependency for testing
            builder.ConfigureServices(services =>
            {
                // Remove the application's existing AppDbContext registration.
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.RemoveAll<AppDbContext>();

                // Register AppDbContext using the test SQLite connection.
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                });
            });

        }

        //4. Create test database schema as table doesn't exist yet
        public async Task InitializeDatabaseAsync()
        {
            using var scope = Services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            //creates the tables from EF Core model, including the Identity tables
            await context.Database.EnsureCreatedAsync(); 
        }

        //5. Dispose the connection
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _connection.Dispose();
            }
        }
    }
}
