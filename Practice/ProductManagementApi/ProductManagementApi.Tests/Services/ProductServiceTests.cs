
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

    #region GET
    
    #region GetByID
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

    #endregion

    #region GetAll
    //Test 1: Return all Products
    [Fact]
    public async Task GetAllAsync_WhenProductsExist_ReturnsAllProducts()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product
            {
                ProductName = "Mobile",
                Price = 500,
                StockLevel = 10
            },
            new Product
            {
                ProductName = "Laptop",
                Price = 1200,
                StockLevel = 5
            },
            new Product
            {
                ProductName = "Charger",
                Price = 100,
                StockLevel = 20
            }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, null, false, 1, 10);

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    //Test2: Search products by name (SEARCH)
    [Fact]
    public async Task GetAllAsync_WhenSearchIsProvided_ReturnsMatchingProducts()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 },
            new Product { ProductName = "Mobile Cover", Price = 50, StockLevel = 15 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            "Mobile", null, false, 1, 10);

        // Assert
        Assert.Equal(2, result.TotalCount); //here check TotalCount for total number of matching products before applying pagination
        Assert.Equal(2, result.Items.Count);

        //This checks that every returned product contains "Mobile" in its name.
        Assert.All(result.Items, product =>
            Assert.Contains("Mobile", product.ProductName));
    }

    //Test3: Search without considering uppercase/lowercase (SEARCH)
    [Fact]
    public async Task GetAllAsync_WhenSearchIsLowercase_ReturnsMatchingProducts()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Mobile Cover", Price = 50, StockLevel = 15 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            "mobile", null, false, 1, 10);  //here small letter m in mobile for test-insensitive searching

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);

        Assert.All(result.Items, product =>
            Assert.Contains("mobile", product.ProductName,
                StringComparison.OrdinalIgnoreCase)); //this line ignore cases.
    }

    // Test4: SORT products by name (Ascending)
    [Fact]
    public async Task GetAllAsync_WhenSortingByNameAscending_ReturnsProductsInAscendingOrder()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, "productname", false, 1, 10); //here false is ascending order

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("Charger", result.Items[0].ProductName);  //this is order as C comes before L and M
        Assert.Equal("Laptop", result.Items[1].ProductName);
        Assert.Equal("Mobile", result.Items[2].ProductName);
    }


    //Test5: SORT products by name (Descending)
    [Fact]
    public async Task GetAllAsync_WhenSortingByNameDescending_ReturnsProductsInDescendingOrder()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, "productname", true, 1, 10);  //here true means descending order

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("Mobile", result.Items[0].ProductName);  //checking if M coming first
        Assert.Equal("Laptop", result.Items[1].ProductName);
        Assert.Equal("Charger", result.Items[2].ProductName);
    }


    //Test6: SORT products by price (Ascending)
    [Fact]
    public async Task GetAllAsync_WhenSortingByPriceAscending_ReturnsProductsInAscendingOrder()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, "price", false, 1, 10);

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("Charger", result.Items[0].ProductName);
        Assert.Equal("Mobile", result.Items[1].ProductName);
        Assert.Equal("Laptop", result.Items[2].ProductName);

        Assert.Equal(100, result.Items[0].Price);
        Assert.Equal(500, result.Items[1].Price);
        Assert.Equal(1200, result.Items[2].Price);
    }


    //Test7: SORT products by price (Descending)
    [Fact]
    public async Task GetAllAsync_WhenSortingByPriceDescending_ReturnsProductsInDescendingOrder()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, "price", true, 1, 10);

        // Assert
        Assert.Equal(3, result.Items.Count);

        Assert.Equal("Laptop", result.Items[0].ProductName);
        Assert.Equal("Mobile", result.Items[1].ProductName);
        Assert.Equal("Charger", result.Items[2].ProductName);

        Assert.Equal(1200, result.Items[0].Price);
        Assert.Equal(500, result.Items[1].Price);
        Assert.Equal(100, result.Items[2].Price);
    }

    //Test8: (PAGINATION) Return the correct products for a requested page 
    [Fact]
    public async Task GetAllAsync_WhenSecondPageRequested_ReturnsCorrectProducts()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 },
            new Product { ProductName = "Keyboard", Price = 300, StockLevel = 8 },
            new Product { ProductName = "Mouse", Price = 200, StockLevel = 12 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, null, false, 2, 2); //page 2 and 2 products per page

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(3, result.TotalPages);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Charger", result.Items[0].ProductName);
        Assert.Equal("Keyboard", result.Items[1].ProductName);
    }

    //Test9: (PAGINATION) Handle page 0 and page size 0 , invalid pagination values
    [Fact]
    public async Task GetAllAsync_WhenInvalidPaginationProvided_UsesMinimumValues()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = new List<Product>
        {
            new Product { ProductName = "Mobile", Price = 500, StockLevel = 10 },
            new Product { ProductName = "Laptop", Price = 1200, StockLevel = 5 },
            new Product { ProductName = "Charger", Price = 100, StockLevel = 20 }
        };

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, null, false, 0, 0); //0 page, 0 Page size but Math.Clamp prevents this as it has range between 1 to 100

        // Assert
        Assert.Equal(1, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(1, result.Items.Count);
        Assert.Equal(3, result.TotalPages);
    }

    //Test10: (PAGINATION) Page size cannot exceed 100
    [Fact]
    public async Task GetAllAsync_WhenPageSizeExceedsMaximum_LimitsPageSizeTo100()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var products = Enumerable.Range(1, 105) //this create out of range 105 product 
            .Select(i => new Product
            {
                ProductName = $"Product {i}",
                Price = i * 10,
                StockLevel = i
            })
            .ToList();

        database.Context.Products.AddRange(products);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.GetAllAsync(
            null, null, false, 1, 150);

        // Assert
        Assert.Equal(1, result.Page);
        Assert.Equal(100, result.PageSize);
        Assert.Equal(105, result.TotalCount);
        Assert.Equal(100, result.Items.Count);
        Assert.Equal(2, result.TotalPages);
    }

    #endregion

    #endregion

    #region POST

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

    #endregion

    #region PUT

    [Fact]
    public async Task UpdateAsync_WhenProductExists_UpdatesProduct()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var product = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        database.Context.Products.Add(product); //first save original product to test update
        await database.Context.SaveChangesAsync();

        var dto = new UpdateProductDto
        {
            ProductName = "Smartphone",
            Price = 700,
            StockLevel = 15
        };

        // Act
        var result = await service.UpdateAsync(product.Id, dto);

        // Assert
        Assert.True(result);

        //checking product is actually updated
        var updatedProduct = await database.Context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        Assert.NotNull(updatedProduct);
        Assert.Equal("Smartphone", updatedProduct.ProductName);
        Assert.Equal(700, updatedProduct.Price);
        Assert.Equal(15, updatedProduct.StockLevel);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var dto = new UpdateProductDto
        {
            ProductName = "Laptop",
            Price = 1200,
            StockLevel = 5
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductNameAlreadyExists_ThrowsDuplicateException()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var product1 = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        var product2 = new Product
        {
            ProductName = "Laptop",
            Price = 1200,
            StockLevel = 5
        };

        database.Context.Products.AddRange(product1, product2);
        await database.Context.SaveChangesAsync();

        var dto = new UpdateProductDto  //updating product with duplicate name
        {
            ProductName = "mobile",
            Price = 1300,
            StockLevel = 8
        };

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateProductNameException>(
            () => service.UpdateAsync(product2.Id, dto));
    }

    #endregion

    #region PATCH

    [Fact]
    public async Task PatchAsync_WhenOnlyPriceIsProvided_UpdatesOnlyPrice()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var product = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        database.Context.Products.Add(product);
        await database.Context.SaveChangesAsync();

        var dto = new PatchProductDto
        {
            Price = 700
        };

        // Act
        var result = await service.PatchAsync(product.Id, dto);

        // Assert
        Assert.True(result);

        var updatedProduct = await database.Context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        Assert.NotNull(updatedProduct);

        Assert.Equal("Mobile", updatedProduct.ProductName);
        Assert.Equal(700, updatedProduct.Price);
        Assert.Equal(10, updatedProduct.StockLevel);
    }

    [Fact]
    public async Task PatchAsync_WhenProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var dto = new PatchProductDto
        {
            Price = 700
        };

        // Act
        var result = await service.PatchAsync(999, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task PatchAsync_WhenProductNameAlreadyExists_ThrowsDuplicateException()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var product1 = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        var product2 = new Product
        {
            ProductName = "Laptop",
            Price = 1200,
            StockLevel = 5
        };

        database.Context.Products.AddRange(product1, product2);
        await database.Context.SaveChangesAsync();

        var dto = new PatchProductDto
        {
            ProductName = "mobile"
        };

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateProductNameException>(
            () => service.PatchAsync(product2.Id, dto));
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task DeleteAsync_WhenProductExists_DeletesProduct()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        var product = new Product
        {
            ProductName = "Mobile",
            Price = 500,
            StockLevel = 10
        };

        database.Context.Products.Add(product);
        await database.Context.SaveChangesAsync();

        // Act
        var result = await service.DeleteAsync(product.Id);

        // Assert
        Assert.True(result);

        var deletedProduct = await database.Context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        using var database = new TestDatabase();

        var logger = new Mock<ILogger<ProductService>>();
        var service = new ProductService(database.Context, logger.Object);

        // Act
        var result = await service.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }

    #endregion
}
