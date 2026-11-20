
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductManagementApi.Tests.Helpers;

public class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public AppDbContext Context { get; }

    public TestDatabase()
    {
        //stores data temporarily in EF Core's InMemory (:memory)
        _connection = new SqliteConnection("Data Source=:memory:");

        //SQLite in-memory databases exist only while their connection remains open.
        _connection.Open(); 

        //Configure EF Core to use temporary SQLite database instead of actual products.db
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        //Create context manually with test specific options
        Context = new AppDbContext(options); //supplying the dependency ourselves rather than using the application's normal DI container.

        //Create database tables/ schema from our EF core model
        Context.Database.EnsureCreated(); //for this test, no need to run migrations
    }

    //Dispose resources, clean up database and connection when test finishes
    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
