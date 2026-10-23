using Microsoft.AspNetCore.Mvc;
using PracticeSandBox.Models;
using PracticeSandBox.Data;

namespace PracticeSandBox.Controllers
{
    public class SandBoxController : Controller
    {
        private readonly AppDbContext _context;

        //Inject database context via constructor
        public SandBoxController(AppDbContext context)
        {
            _context = context;

            //This creates sandbox.db automatically if doesn't exist
            _context.Database.EnsureCreated();
        }

        //SandBox/Index
        public IActionResult Index()
        {
            // creating ViewModel and populating with data
            var viewModel = new ProductDetailViewModel
            {
                ProductName = "Acer Laptop",
                Price = 100m,
                StockLevel =1,
                IsOnSale = true,
            };

            // pass viewModel to View method
            return View(viewModel); 
        }

        // GET: /SandBox/Update
        [HttpGet]
        public IActionResult Update()
        {
            var model = new ProductUpdateViewModel
            {
                ProductName = "Acer Laptop 10",
                Price = 10m,
                StockLevel = 10,
            };

            return View(model);
        }

        // POST: /SandBox/Update
        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel model)
        {
            //if invalid, return same view 
            if (!ModelState.IsValid) return View(model);

            // Create a new database entity from form data
            var product = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                StockLevel = model.StockLevel,
            };

            // Add to context and save changes to SqlLite database
            _context.Products.Add(product);
            _context.SaveChanges();
            
            // if valid, process
            model.SuccessMessage = $"Successfully update '{model.ProductName}'";

            return View(model);
        }
    }
}
