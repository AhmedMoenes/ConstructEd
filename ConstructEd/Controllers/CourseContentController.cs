using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Controllers
{
    public class CourseContentController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseContentController(
            ICourseContentRepository courseContentRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        // GET: CourseContent
        public async Task<IActionResult> Index()
        {
            var courseContents = await _courseContentRepository.GetAllAsync();
            var viewModels = _mapper.Map<List<CourseContentViewModel>>(courseContents);
            return View(viewModels);
        }

        // GET: CourseContent/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CourseContentViewModel();
            ViewBag.Courses = await _courseRepository.GetAllAsync();
            return View((nameof(Create), viewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseContent = _mapper.Map<CourseContent>(viewModel);
                await _courseContentRepository.InsertAsync(courseContent);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = await _courseRepository.GetAllAsync();
            return View(nameof(Create), viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }

            var courses = await _courseRepository.GetAllAsync();
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);
            viewModel.Courses = courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title,
                Selected = c.Id == courseContent.CourseId
            });
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseContentViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var courseContent = _mapper.Map<CourseContent>(viewModel);
                await _courseContentRepository.UpdateAsync(courseContent);
                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, repopulate the courses dropdown and return the view
            var courses = await _courseRepository.GetAllAsync();
            viewModel.Courses = courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title,
                Selected = c.Id == viewModel.CourseId
            });
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseContentRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}