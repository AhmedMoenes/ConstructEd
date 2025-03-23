using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        public AccountController(UserManager<ApplicationUser> userManager,
                         SignInManager<ApplicationUser> signInManager,  // ✅ Add this
                         IAuthService authService,
                         IMapper mapper,
                         IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _userManager = userManager;
            _signInManager = signInManager; // ✅ Assign it
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(nameof(Register));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _authService.RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    // Automatically login after registration
                    await _authService.LoginUserAsync(new LoginViewModel
                    {
                        Email = model.Email,
                        Password = model.Password
                    });

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(nameof(Register), model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "User,Instructor,Admin")]
        public async Task<IActionResult> Profile()
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null) return NotFound();

            var profileViewModel = _mapper.Map<ProfileViewModel>(user);
            // Fetch enrollments separately and map them
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsByUserIdAsync(user.Id);
            profileViewModel.Enrollments = _mapper.Map<List<EnrollmentViewModel>>(enrollments);

            return View(profileViewModel);
        }

        // To Do : Forget Password, Reset Password  , External Login Via Google, Facebook ## //

    }
}
