using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IInstructorRepository _instructorRepository;
        public InstructorController(UserManager<ApplicationUser> userManager,
                                    IAuthService authService, IMapper mapper,
                                    IEnrollmentRepository enrollmentRepository,
                                    IInstructorRepository instructorRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _userManager = userManager;
            _authService = authService;
            _mapper = mapper;
            _instructorRepository = instructorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return View(instructors);
        }
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    // Define the folder to save the image
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image");

                    // Generate a unique file name
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.ImageFile.FileName);
                    var filePath = Path.Combine(imagesFolder, fileName);

                    // Ensure the folder exists
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    // Save the file name to the Image property
                    viewModel.ProfilePicture = fileName;
                }
                IdentityResult result = await _authService.RegisterInstructorAsync(viewModel);

                if (result.Succeeded)
                {
                    return RedirectToAction("Manage", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(nameof(Create), viewModel);
        }
    }
}