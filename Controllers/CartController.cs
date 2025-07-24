using iniskincare.Domain.Auth;
using iniskincare.Domain.Cart;
using iniskincare.Domain.Entities;
using iniskincare.Domain.Interfaces.Cart;
using iniskincare.Domain.IProduct;
using iniskincare.Models;
using Microsoft.AspNetCore.Mvc;

namespace iniskincare.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IUserService userService,IProductService productService)
        {
            _cartService = cartService;
            _userService = userService;
            _productService = productService;
        }

        public async Task<IActionResult> Cart()
        {
                var email = HttpContext.Session.GetString("UserEmail");
                var user = await _userService.GetByEmailAsync(email);

                if (user == null)
                    return RedirectToAction("Profile", "Auth");

                var items = await _cartService.GetCartItems(user.Id);
                return View(items);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                var email = HttpContext.Session.GetString("UserEmail");
                var user = await _userService.GetByEmailAsync(email);
                var product = await _productService.GetByIdAsync(productId);

                if (user == null || product == null)
                    return NotFound();

                await _cartService.AddToCart(user.Id, email, product);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                // Known, expected issues (e.g., invalid product). Return 400.
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log exception (use ILogger)
                // Log.LogError(ex, "Error in AddToCart");

                // Return generic 500 status with safe payload
                return StatusCode(500, new { message = "Something went wrong. Please try again later." });
            }
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
