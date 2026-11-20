
using Microsoft.Extensions.Logging;
using Moq;
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
}
