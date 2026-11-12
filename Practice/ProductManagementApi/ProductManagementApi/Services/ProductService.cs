using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Exceptions;

namespace ProductManagementApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;  //Logging service

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public ProductResponseDto ToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price,
            StockLevel = product.StockLevel
        };
    }

    //IMPLEMENTATION----------------
    public async Task<ProductListResponseDto> GetAllAsync(
    string? search, string? sortBy, bool descending, int page,
    int pageSize)
    {
        // 1. Start building the database query
        var query = _context.Products.AsQueryable();

        // 2. Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.ProductName.Contains(search));
        }

        // 3. Validate pagination values
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var totalCount = await query.CountAsync();

        // 4. Sorting
        var sort = sortBy?.ToLowerInvariant();

        List<Product> products;

        if (sort == "price")
        {
            // SQLite decimal sorting workaround
            var matchingProducts = await query.ToListAsync();

            var sortedProducts = descending
                ? matchingProducts
                    .OrderByDescending(p => p.Price)
                    .ThenBy(p => p.Id)
                : matchingProducts
                    .OrderBy(p => p.Price)
                    .ThenBy(p => p.Id);

            // 5. Pagination after sorting
            products = sortedProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        else
        {
            query = (sort, descending) switch
            {
                ("productname", false) => query.OrderBy(p => p.ProductName),
                ("productname", true) => query.OrderByDescending(p => p.ProductName),
                ("stocklevel", false) => query.OrderBy(p => p.StockLevel),
                ("stocklevel", true) => query.OrderByDescending(p => p.StockLevel),
                (_, false) => query.OrderBy(p => p.Id),
                (_, true) => query.OrderByDescending(p => p.Id)
            };

            // Pagination in database
            products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // 6. Convert entities to response DTOs
        var result = products.Select(ToResponseDto).ToList();

        // 7. Build response
        return new ProductListResponseDto
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize),
            Items = result
        };
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var nameExists = await _context.Products
                .AnyAsync(p => p.ProductName.ToLower() == dto.ProductName.ToLower());

        //Since service knows product name is duplicated checked, so no need to check HTTP status code 409 
        if (nameExists)
        {
            throw new DuplicateProductNameException();
        }

        var product = new Product
        {
            ProductName = dto.ProductName,
            Price = dto.Price,
            StockLevel = dto.StockLevel
        };

        _context.Products.Add(product);

        // Datebase level Exception Handling with helper method for duplicate
        try
        {
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Prodcuts {ProductID} was created successfully.",
                product.Id
                );
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateProductNameException();
        }

        return ToResponseDto(product);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        var nameExists = await _context.Products
                .AnyAsync(p =>
                    p.Id != id &&
                    p.ProductName.ToLower() == dto.ProductName.ToLower());

        if (nameExists)
        {
            throw new DuplicateProductNameException();
        }

        //Update Product Properties
        product.ProductName = dto.ProductName;
        product.Price = dto.Price;
        product.StockLevel = dto.StockLevel;

        //Save changes and handle database constraint
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateProductNameException();
        }

        return true;
    }


    // Handle database unique-constraint exceptions
    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqliteException sqliteException
            && sqliteException.SqliteExtendedErrorCode == 2067;
    }
}