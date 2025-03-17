using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public CourseController(ICourseRepository courseRepository,
                                IInstructorRepository instructorRepository,
                                ICourseContentRepository courseContentRepository,
                                IWishListRepository wishlistRepository,
                                IShoppingCartRepository shoppingCartRepository,
                                IMapper mapper)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
            _courseContentRepository = courseContentRepository;
            _wishlistRepository = wishlistRepository;
            _shoppingCartRepository= shoppingCartRepository;
            _mapper = mapper;
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
                    course.IsInCart = await _shoppingCartRepository.IsCourseInCartAsync(userId, course.Id); // ✅ New check
                }
            }

            return View(nameof(Index), courseViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseDetailsViewModel>(course);
            return View(nameof(Details), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManageContent(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseViewModel>(course);
            return View(nameof(ManageContent), viewModel);
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

        [HttpPost, ActionName("Remove")]
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
    }
}