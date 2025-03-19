using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
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
        private readonly IMapper _mapper;

        public AccountController( UserManager<ApplicationUser> userManager
                            , IAuthService authService, IMapper mapper
                           , IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _userManager =userManager;
            _authService = authService;
            _mapper = mapper;
        }

        public IActionResult Manage()
        {
            return View(nameof(Manage));
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

                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
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
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
            }
            return View(nameof(Login), model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

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
