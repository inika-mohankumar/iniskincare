using Microsoft.AspNetCore.Mvc;

namespace iniskincare.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Products()
        {
            return View();
        }
    }
}
