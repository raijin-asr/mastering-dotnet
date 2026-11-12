using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.DTOs;
using Microsoft.Data.Sqlite;
using ProductManagementApi.Services;
using ProductManagementApi.Exceptions;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context; //will remove this later

        private readonly IProductService _productService; //Inject service instead of AppDbContext      

        private readonly ILogger<ProductsController> _logger;  //Logging service


        // constructor dependency injection
        public ProductsController(IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
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
            
            var response = await _productService.GetAllAsync(
                search, sortBy, descending, page, pageSize);

            return Ok(response);
        }



        [HttpGet("{id:int}")]  // -> /api/Products/id
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id); //comunicate with service instead of directly to dbcontext

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

            return Ok(_productService.ToResponseDto(product));
        }

        [HttpPost]  //api/products
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
        {
            try
            {
                var response = await _productService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = response.Id },
                    response
                );

            }
            catch (DuplicateProductNameException ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name"
                    );
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductDto dto)
        {
            try
            {
                var updated = await _productService.UpdateAsync(id, dto);

                if (!updated)
                {
                    return Problem(
                        detail: $"Product with ID {id} was not found.",
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Product Not Found");
                }

                return NoContent();
            }
            catch (DuplicateProductNameException ex) 
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Duplicate Product Name"
                    );
            }
                
                
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
