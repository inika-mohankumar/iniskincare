using iniskincare.Data;
using Microsoft.AspNetCore.Mvc;

namespace iniskincare.Controllers
{
    public class ProductController : Controller
    {
        private readonly IniskincareDbContext _context;

        public ProductController(IniskincareDbContext context)
        {
            _context = context;
        }
        public IActionResult Products(string searchTerm)
        {
            List<string> categories = new List<string>
            {
              "Body Care",
              "Face Care",
              "Lip Care",
              "Hair Care",
              "Sun Protection",
              "All"
            };
            ViewBag.Categories = categories;

            var allProducts = _context.Product.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                allProducts = allProducts
                    .Where(p => p.Name.ToLower().Contains(searchTerm))
                    .ToList();
            }

            return View(allProducts);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Product.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
    }
}
