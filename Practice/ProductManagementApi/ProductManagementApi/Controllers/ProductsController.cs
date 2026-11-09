using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.DTOs;
using Microsoft.Data.Sqlite;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;  //Logging service

        //mapping method to convert a database Product entity into the response DTO
        private static ProductResponseDto ToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockLevel = product.StockLevel
            };
        }

        // constructor dependency injection
        public ProductsController(AppDbContext context,
            ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<ProductListResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? sortBy,
            [FromQuery] bool descending = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
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

            // Count matching products before pagination
            var totalCount = await query.CountAsync();

            // 4. Sorting
            var sort = sortBy?.ToLowerInvariant();

            List<Product> products;

            if (sort == "price")
            {
                // SQLite decimal sorting workaround:
                // Sort matching records in C#.
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

                // Pagination happens in the database
                products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }

            // 6. Convert entities to response DTOs
            var result = products.Select(ToResponseDto).ToList();

            // 7. Return products and pagination metadata
            var response = new ProductListResponseDto
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)pageSize),
                Items = result
            };

            return Ok(response);
        }



        [HttpGet("{id:int}")]  // -> /api/Products/id
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                //Add logger message
                _logger.LogWarning(
                    "Product with ID {ProductId} was not found.",
                    id);

                //Creating consistent 404 response
                return Problem(
                    detail: $"Product with ID {id} was not found",
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Product Not Found"
                    );
            }

            return Ok(ToResponseDto(product));
        }

        [HttpPost]  //api/products
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
        {
            var nameExists = await _context.Products
                .AnyAsync(p => p.ProductName.ToLower() == dto.ProductName.ToLower());

            //If it exists, return 409 Conflict. This is ordinary duplicate check
            if (nameExists)
            {
                return Problem(
                    detail: "A product with this name already exists.",
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name");
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
                when(IsUniqueConstraintViolation(ex))
            {
                return Problem(
                    detail: "A product with this name already exists.",
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name");
            }

            var response = ToResponseDto(product);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                response
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductDto dto)
        {
            var nameExists = await _context.Products
                .AnyAsync(p =>
                    p.Id != id &&
                    p.ProductName.ToLower() == dto.ProductName.ToLower());

            if (nameExists)
            {
                return Problem(
                    detail: "A product with this name already exists.",
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name");
            }

            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.StockLevel = dto.StockLevel;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (IsUniqueConstraintViolation(ex))
            {
                return Problem(
                    detail: "A product with this name already exists.",
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, PatchProductDto dto)
        {
            if (dto.ProductName is not null)
            {
                var nameExists = await _context.Products
                    .AnyAsync(p =>
                        p.Id != id &&
                        p.ProductName.ToLower() == dto.ProductName.ToLower());

                if (nameExists)
                {
                    return Conflict(new
                    {
                        message = "A product with this name already exists."
                    });
                }
            }

            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            if (dto.ProductName is not null)
            {
                product.ProductName = dto.ProductName;
            }

            if (dto.Price.HasValue)
            {
                product.Price = dto.Price.Value;
            }

            if (dto.StockLevel.HasValue)
            {
                product.StockLevel = dto.StockLevel.Value;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (IsUniqueConstraintViolation(ex))
            {
                return Conflict(new
                {
                    message = "A product with this name already exists."
                });
            }

            return NoContent();
        }

        // Handle database unique-constraint exceptions
        private static bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            return exception.InnerException is SqliteException sqliteException
                && sqliteException.SqliteExtendedErrorCode == 2067;
        }


    }
}
