using iniskincare.Data;
using iniskincare.Helpers;
using iniskincare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace iniskincare.Controllers
{
    public class AuthController : Controller
    {
        private readonly IniskincareDbContext _context;

        public AuthController(IniskincareDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user); 

            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }

                user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View(user); 
            }
        }

        public IActionResult Profile()
        {
            return View("Profile"); 
        }


        [HttpPost]
        public IActionResult Profile(string email, string password)
        {
            try
            {
                var hashed = HashPassword(password);
                var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hashed);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                  
                    return View();
                }

                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                

                ModelState.AddModelError(string.Empty, "An error occurred while trying to log in. Please try again.");
                
                return View() ;
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Profile", "Auth");
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
