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

    }
}