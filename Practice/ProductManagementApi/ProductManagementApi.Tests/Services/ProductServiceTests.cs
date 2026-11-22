
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagementApi.DTOs;
using ProductManagementApi.Exceptions;
using ProductManagementApi.Models;
using ProductManagementApi.Services;
using ProductManagementApi.Tests.Helpers;

namespace ProductManagementApi.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        // 1. Arrange--------------
        using var database = new TestDatabase();//creates temporary database

        //create mock logger
        var logger = new Mock<ILogger<ProductService>>(); //just providing it becuase this dependency required by service

        //create Product Service, manually injecting dependencies
        var service = new ProductService(
            database.Context, //passing context of database
            logger.Object); //passing object of logger to constructor

        //Insert test data
        var product = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        database.Context.Products.Add(product);
        await database.Context.SaveChangesAsync();

        // 2. Act-----------------
        var result = await service.GetByIdAsync(product.Id); //execute exisiting action method

        // 3. Assert ------------

        //verifying resut
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("Mobile", result.ProductName);
        Assert.Equal(500, result.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();

        var service = new ProductService(
            database.Context,
            logger.Object);

        // Act
        var result = await service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WhenProductIsValid_CreatesProduct()
    {
        // 1. Arrange
        using var database = new TestDatabase(); //create a separate own test database

        var logger = new Mock<ILogger<ProductService>>(); //create a logger

        var service = new ProductService(  //create a service, injecting dependencies manually
            database.Context,
            logger.Object
            );

        var dto = new CreateProductDto  //preapare a test dto
        {
            ProductName = "GTA VI",
            Price = 10000,
            StockLevel = 1
        };

        // 2. Act
        var result = await service.CreateAsync(dto);

        // 3. Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("GTA VI", result.ProductName);
        Assert.Equal(10000, result.Price);
        Assert.Equal(1, result.StockLevel);

        //Verify that proudct was actually saved
        var savedProduct = await database.Context.Products
            .FirstOrDefaultAsync(p => p.Id == result.Id);

        Assert.NotNull(savedProduct);
        Assert.Equal("GTA VI", savedProduct.ProductName);
    }


    [Fact]
    public async Task CreateAsync_WhenProductNameAlreadyExists_ThrowsDuplicateException()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();

        var service = new ProductService(
            database.Context,
            logger.Object);

        var existingProduct = new Product  //Original Product
        {
            ProductName = "GTA VI",
            Price = 10000,
            StockLevel = 1
        };

        database.Context.Products.Add(existingProduct);
        await database.Context.SaveChangesAsync();

        var dto = new CreateProductDto  //new duplicate product
        {
            ProductName = "gta vi",
            Price = 600,
            StockLevel = 15
        };

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateProductNameException>(
            () => service.CreateAsync(dto)); //Exception Assertion
    }

    [Fact]
    public async Task Database_WhenDuplicateProductNameIsInserted_ThrowsUniqueConstraintViolation()
    {
        // Arrange
        using var database = new TestDatabase();

        var product1 = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        database.Context.Products.Add(product1);
        await database.Context.SaveChangesAsync(); //this insert Mobile

        var product2 = new Product
        {
            ProductName = "mobile",
            Price = 600,
            StockLevel = 15
        };

        database.Context.Products.Add(product2);

        // Act
        var exception = await Assert.ThrowsAsync<DbUpdateException>(
            () => database.Context.SaveChangesAsync()); //here trying to insert mobile to test duplicate
        //but sqlite reject this 2nd insert as unique index uses NOCASE in AppDbContext and treat it duplicate

        // Assert
        var sqliteException = Assert.IsType<SqliteException>(
            exception.InnerException); //EF core shows DpUpdateException

        
        Assert.Equal(2067, sqliteException.SqliteExtendedErrorCode);  //check 2067 represents a unique contraint violation
    }

}
