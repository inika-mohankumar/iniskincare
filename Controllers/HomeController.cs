using iniskincare.Data;
using iniskincare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace iniskincare.Controllers
{
    public class HomeController : Controller
    {
        private readonly IniskincareDbContext _context;

        public HomeController(IniskincareDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("UserEmail");
            ViewBag.UserEmail = email;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public IActionResult About() => View();
      

    }
}
