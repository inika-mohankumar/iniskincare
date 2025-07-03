using Microsoft.AspNetCore.Mvc;
using iniskincare.Domain.Entities;
using iniskincare.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace iniskincare.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var success = await _userService.RegisterAsync(user);

            if (!success)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(user);
            }

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Profile() => View();

        [HttpPost]
        public async Task<IActionResult> Profile(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userService.LoginAsync(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Profile");
        }
    }
}
