using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register"); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Create user logic
                return RedirectToAction("Login"); // Redirects to the Login action
            }
            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Returns Views/Account/Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Login logic
                return RedirectToAction("Index", "Home"); // Redirects to the Home/Index action
            }
            return View(model); // Returns Views/Account/Login.cshtml with validation errors
        }

        public async Task<IActionResult> Logout(LoginViewModel loginModel)
        {
            return RedirectToAction("Index", "Home");
        }

        // To Do : Forget Password, Reset Password  , External Login Via Google, Facebook ## //z

    }
}
