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
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            var result = products.Select(ToResponseDto).ToList();

            return Ok(result);
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
