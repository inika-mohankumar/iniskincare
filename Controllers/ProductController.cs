
using iniskincare.Domain.IProduct;
using Microsoft.AspNetCore.Mvc;

namespace iniskincare.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Products(string searchTerm)
        {
            ViewBag.Categories = _productService.GetCategories();
            var products = await _productService.GetFilteredProductsAsync(searchTerm);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
