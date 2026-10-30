using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.DTOs;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

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
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]  // /api/Products
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll(
            [FromQuery] string? search, //read serach value from URL query string as FromBody tells where value comes from
            [FromQuery] string? sortBy,
            [FromQuery] bool descending = false
            )
        {
            // starts building database query but doesn't get data immediately
            var query = _context.Products.AsQueryable();// no await as it has no GetAwaiter method

            //Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.ProductName.Contains(search));

                //also can be used LIKE to handle ToLower()
                //query = query.Where(p => EF.Functions.Like(p.ProductName, $"%{search}%"));

            }

            // Sorting
            var sort = sortBy?.ToLowerInvariant();

            if (sort == "price")
            {
                // SQLite doesn't support ORDER BY decimal through this EF Core provider.
                // Retrieve the filtered records, then sort them in C#.
                var products = await query.ToListAsync();

                products = descending
                    ? products.OrderByDescending(p => p.Price).ToList()
                    : products.OrderBy(p => p.Price).ToList();

                return Ok(products.Select(ToResponseDto).ToList());
            }

            query = (sort, descending) switch
            {
                ("productname", false) => query.OrderBy(p => p.ProductName),
                ("productname", true) => query.OrderByDescending(p => p.ProductName),
                ("stocklevel", false) => query.OrderBy(p => p.StockLevel),
                ("stocklevel", true) => query.OrderByDescending(p => p.StockLevel),
                (_, false) => query.OrderBy(p => p.Id),
                (_, true) => query.OrderByDescending(p => p.Id)
            };

            var sortedProducts = await query.ToListAsync();

            return Ok(sortedProducts.Select(ToResponseDto).ToList());
        }


        [HttpGet("{id:int}")]  // -> /api/Products/id
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(ToResponseDto(product));
        }

        [HttpPost]  //api/products
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                Price = dto.Price,
                StockLevel = dto.StockLevel
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

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
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.StockLevel = dto.StockLevel;

            await _context.SaveChangesAsync();

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
    }
}
