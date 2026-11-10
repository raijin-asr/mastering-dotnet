using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.DTOs;
using ProductManagementApi.Models;

namespace ProductManagementApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
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
}