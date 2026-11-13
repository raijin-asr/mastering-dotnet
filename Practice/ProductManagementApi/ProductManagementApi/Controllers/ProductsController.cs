using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs;
using ProductManagementApi.Exceptions;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

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
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return Problem(
                    detail: $"Product with ID {id} was not found.",
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Product Not Found");
            }

            _logger.LogInformation(
                "Product {ProductId} was deleted successfully.",
                id);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, PatchProductDto dto)
        {

            try
            {
                var updated = await _productService.PatchAsync(id, dto);

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

    }
}
