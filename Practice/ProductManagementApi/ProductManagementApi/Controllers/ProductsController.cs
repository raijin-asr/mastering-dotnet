using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //without Database, hardcoded Product data practice first using in-memory list
        public static readonly List<Product> _products = new() //store our temporary product data in memory, here static let all controller of this class, not each individual share same list while application is running
        {
            new Product //this new possible because list is mutable
            {
                Id = 1,
                ProductName = "Mobile",
                Price = 19,
                StockLevel = 10
            },
            new Product
            {
                Id = 2,
                ProductName = "Charger",
                Price = 5,
                StockLevel = 20
            },
        };

        [HttpGet]  // /api/Products
        public ActionResult<IEnumerable<Product>> GetAll() //here IEnumerable for read-only uneditable data, as IEnumerable<T> is the base interface for almost all collections in .NET. It doesn't store data itself; it simply represents a sequence of data that you can loop through (using a foreach loop).
        {
            return Ok(_products); //returns HTTP 200 OK with the products in the response body.
        }

        [HttpGet("{id:int}")]  // -> /api/Products/id
        public ActionResult<Product> GetById(int id) 
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if(product is null) return NotFound();

            return Ok(product);
        }

        [HttpPost]  //api/products
        public ActionResult<Product> Create(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1; // manually increase id by

            _products.Add(product);

            return CreatedAtAction( // returns HTTP 201 Created, along with the created product and a URL pointing to its GetById endpoint.
                nameof(GetById),
                new { id = product.Id },
                product
                );

        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Product updatedProduct) //id is from URL to identify product
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            // Do not replace existing product's ID with the ID from the JSON body
            product.ProductName = updatedProduct.ProductName;
            product.Price = updatedProduct.Price;
            product.StockLevel = updatedProduct.StockLevel;

            return NoContent(); //returns HTTP 204 to indicate that the update succeeded without a response body.
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }
            
            //here since its in-memory list, no need of JSON body as ID of url identifies product to delete
            _products.Remove(product);

            return NoContent();
        }
    }
}
