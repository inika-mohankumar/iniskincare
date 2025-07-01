using iniskincare.Data;
using iniskincare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iniskincare.Controllers
{
    public class CartController : Controller
    {
        private readonly IniskincareDbContext _context;

        public CartController(IniskincareDbContext context)
        {
            _context = context;
        }
        public IActionResult Cart()
        {

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
             
                return RedirectToAction("Profile", "Auth");
            }


            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            string email = HttpContext.Session.GetString("UserEmail");
            if (email == null)
                return Unauthorized();

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);

            if (user == null || product == null)
                return NotFound();

            var cartItem = _context.CartItems.FirstOrDefault(c => c.ProductId == productId && c.UserId == user.Id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    ProductName = product.Name,
                    Quantity = 1,
                    UserEmail = email
                };
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();

            return Ok(); // Very important for JS to detect success
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var item = _context.CartItems.FirstOrDefault(c => c.Id == cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, string action)
        {
            var item = _context.CartItems.First(c => c.Id == cartItemId);
            if (item != null)
            {
                if (action == "increase")
                    item.Quantity++;
                else if (action == "decrease" && item.Quantity > 1)
                    item.Quantity--;

                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(userEmail))
            {
                var items = _context.CartItems
                    .Where(c => c.UserEmail == userEmail)
                    .ToList();

                _context.CartItems.RemoveRange(items);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
    }
}
