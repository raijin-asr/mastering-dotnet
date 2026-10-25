using Microsoft.AspNetCore.Mvc;
using PracticeSandBox.Models;
using PracticeSandBox.Data;
using Microsoft.EntityFrameworkCore;

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
            //_context.Database.EnsureCreated();
        }


        //GET: /SandBox/Index OR /SandBox/?searchString=value&sortOrder=value
        public async Task<IActionResult> Index(
            string? searchString,
            string? sortOrder,
            int page = 1
            ) //or could just use GetAllProducts() method
        {
            const int pageSize = 5;

            //ViewData is a dictionary that lets your controller pass data to its view for the current request.
            ViewData["CurrentSort"] = sortOrder; 
            ViewData["CurrentFilter"] = searchString; //"CurrentFilter" is simply a key that you chose or put anything 

            var products = _context.Products.AsQueryable(); //no await and AsQueryable() lets us build a database query incrementally.

            // 1. Filter products by name
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p =>
                    p.ProductName.ToLower().Contains(searchString.ToLower()));
            }

            // First count matching products before Pagination
            int totalProducts = await products.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalProducts / (double)pageSize);

            //Keep requested page within valid bounds
            if (page < 1) page = 1;

            if (totalPages > 0 && page > totalPages)
                page = totalPages;


            // 3. Sort filtered Products
            // Determine the next sort direction when a heading is clicked
            ViewData["NameSortParm"] =
                sortOrder == "name_asc" ? "name_desc" : "name_asc";

            ViewData["PriceSortParm"] =
                sortOrder == "price_asc" ? "price_desc" : "price_asc";

            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;

                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;

                case "price_asc":
                    products = products.OrderBy(p => (double)p.Price); //here double casting as sqlite does not support decimal price ordering
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => (double)p.Price);
                    break;

                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            // 4. Get only products for requested page
            var pagedProducts = await products.
                 Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

            // 5. Send pagination info to view
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalProducts"] = totalProducts;

            return View(pagedProducts);
        }

        #region Add
        // GET: /SandBox/Add
        [HttpGet]
        public IActionResult Add()
        {
            var model = new ProductFormViewModel
            {
                ProductName = "",
                Price = 0,
                StockLevel = 0,
            };

            return View(model);
        }

        // POST: /SandBox/Add
        [HttpPost]
        public IActionResult Add(ProductFormViewModel model)
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
            model.SuccessMessage = $"Successfully added '{model.ProductName}'";

            return RedirectToAction(nameof(Index)); //redirect to index

            //return View(model);
        }

        #endregion

        #region View
        //Get: /SandBox/Details/id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product is null)
            {
                return NotFound();
            }
            
            return View(product);
        }


        #endregion

        #region Update
        //Get: /SandBox/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return NotFound();

            //here mapping products values to viewModel to pass to HttpPost
            var model = new ProductFormViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockLevel = product.StockLevel
            };

            return View(model);
        }

        //Post: /SandBox/Edit
        [HttpPost]
        [ValidateAntiForgeryToken] //protects POST action against a security issue called Cross-Site Request Forgery (CSRF)
        public async Task<IActionResult> Edit(ProductFormViewModel model) //here model of type ProductFormViewModel has submitted values
        {
            if (!ModelState.IsValid) return View(model);

            var product = await _context.Products.FindAsync(model.Id);
            if (product is null) return NotFound();

            //its existing database entity that changes values
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.StockLevel = model.StockLevel;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();

            _context.Remove(product); //Remove is not asynchronous, so no await as it only mark product for deletion
            await _context.SaveChangesAsync();  //this applies actual deletion to database

            return RedirectToAction(nameof(Index));
        }



        #endregion

    }
}
