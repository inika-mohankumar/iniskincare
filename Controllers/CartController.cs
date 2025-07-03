using iniskincare.Domain.Cart;
using iniskincare.Domain.Entities;
using iniskincare.Domain.Interfaces.Cart;
using iniskincare.Models;
using Microsoft.AspNetCore.Mvc;

namespace iniskincare.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IniskincareDbContext _context;

        public CartController(ICartService cartService, IniskincareDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> Cart()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return RedirectToAction("Profile", "Auth");

            var items = await _cartService.GetCartItems(user.Id);
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);

            if (user == null || product == null)
                return NotFound();

            await _cartService.AddToCart(user.Id, email, product);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCart(cartItemId);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, string action)
        {
            await _cartService.UpdateQuantity(cartItemId, action);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
                await _cartService.ClearCart(email);

            return RedirectToAction("Cart");
        }
    }
}
