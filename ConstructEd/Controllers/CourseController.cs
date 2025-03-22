using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IWishListRepository _wishlistRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper _mapper;
        private readonly IEnrollmentRepository _enrollmentRepository;


        public CourseController(ICourseRepository courseRepository,
                                IInstructorRepository instructorRepository,
                                ICourseContentRepository courseContentRepository,
                                IWishListRepository wishlistRepository,
                                IShoppingCartRepository shoppingCartRepository,
                                IMapper mapper,
                                IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _courseContentRepository = courseContentRepository;
            _wishlistRepository = wishlistRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = await _courseRepository.GetAllAsync();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);

            if (!string.IsNullOrEmpty(userId))
            {
                foreach (var course in courseViewModels)
                {
                    course.IsInWishlist = await _wishlistRepository.IsCourseInWishlistAsync(userId, course.Id);
                    course.IsInCart = await _shoppingCartRepository.IsCourseInCartAsync(userId, course.Id);
                    course.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInCourseAsync(userId, course.Id);
                }
            }
          
            return View(nameof(Index), courseViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseDetailsViewModel>(course);
            viewModel.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInCourseAsync(userId, id);
            viewModel.IsInWishlist = await _wishlistRepository.IsCourseInWishlistAsync(userId, course.Id);
            viewModel.IsInCart = await _shoppingCartRepository.IsCourseInCartAsync(userId, course.Id);
            ViewBag.courseContents = await _courseContentRepository.GetCourseContent(id);

            return View(nameof(Details), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CourseViewModel();
            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();
            return View(nameof(Create), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel viewModel)
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
                    viewModel.Image = fileName;
                }
                try
                {
                    var course = _mapper.Map<Course>(viewModel);
                    await _courseRepository.InsertAsync(course);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();
            return View(nameof(Create), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseViewModel>(course);

            var instructors = await _instructorRepository.GetAllAsync();

            return View(nameof(Edit), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var course = _mapper.Map<Course>(viewModel);
                    await _courseRepository.UpdateAsync(course);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            var instructors = await _instructorRepository.GetAllAsync();

            return View(nameof(Edit), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseViewModel>(course);
            return View(nameof(Remove), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            try
            {
                await _courseRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(Remove), await _courseRepository.GetByIdAsync(id));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesByCategory(Category? category)
        {
            var courses = await _courseRepository.GetByCategoryAsync(category);
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);
            return Json(new { success = true, courses = courseViewModels });
        }
    }
}