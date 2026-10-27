using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // constructor dependency injection
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]  // /api/Products
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }


        [HttpGet("{id:int}")]  // -> /api/Products/id
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]  //api/products
        public async Task<ActionResult<Product>> Create(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction( // returns HTTP 201 Created, along with the created product and a URL pointing to its GetById endpoint.
                nameof(GetById),
                new { id = product.Id },
                product
                );

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            product.ProductName = updatedProduct.ProductName;
            product.Price = updatedProduct.Price;
            product.StockLevel = updatedProduct.StockLevel;

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
