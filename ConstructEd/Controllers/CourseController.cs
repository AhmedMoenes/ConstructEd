using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICourseReviewRepository _courseReviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseRepository courseRepository,
                                IInstructorRepository instructorRepository,
                                ICourseContentRepository courseContentRepository,
                                IWishListRepository wishlistRepository,
                                IShoppingCartRepository shoppingCartRepository,
                                IMapper mapper,
                                UserManager<ApplicationUser> userManager,
                                IEnrollmentRepository enrollmentRepository,
                                ICourseReviewRepository courseReviewRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _courseContentRepository = courseContentRepository;
            _wishlistRepository = wishlistRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _userManager = userManager;
            _enrollmentRepository = enrollmentRepository;
            _courseReviewRepository = courseReviewRepository;
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
            var reviews = await _courseReviewRepository.GetReviewsByCourseIdAsync(id);
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseDetailsViewModel>(course);
            var reviewViewModels = _mapper.Map<List<CourseReviewViewModel>>(reviews);
            viewModel.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInCourseAsync(userId, id);
            viewModel.IsInWishlist = await _wishlistRepository.IsCourseInWishlistAsync(userId, course.Id);
            viewModel.IsInCart = await _shoppingCartRepository.IsCourseInCartAsync(userId, course.Id);
            ViewBag.courseContents = await _courseContentRepository.GetCourseContent(id);
            ViewBag.Reviews = reviewViewModels; 

            return View(nameof(Details), viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddReview(int courseId)
        {
            var viewModel = new CourseReviewViewModel
            {
                CourseId = courseId
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CourseReviewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var review = _mapper.Map<CourseReview>(viewModel);
                review.UserId = _userManager.GetUserId(User);
                review.CreatedAt = DateTime.UtcNow;

                await _courseReviewRepository.AddReviewAsync(review);
                return RedirectToAction(nameof(Details), new { id = viewModel.CourseId });
            }

            return View(viewModel);
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