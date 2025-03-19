using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ConstructEd.Controllers
{
    public class CourseContentController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseContentController(
            ICourseContentRepository courseContentRepository,
            ICourseRepository courseRepository,
            IMapper mapper,
            IEnrollmentRepository enrollmentRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var courseContents = await _courseContentRepository.GetAllAsync();
            var viewModels = _mapper.Map<List<CourseContentViewModel>>(courseContents);

            var courseNames = await _courseContentRepository.GetCourseNamesAsync();
            ViewBag.CourseNames = courseNames;

            return View(viewModels);
        }

        public async Task<IActionResult> Details(int id)

        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            //صاصا 
            
                
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);


            //viewModel.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInCourseAsync(userId, courseContent.CourseId);



            //صاصا

            var courseName = await _courseContentRepository.GetCourseNameByIdAsync(courseContent.CourseId);
            ViewBag.CourseName = courseName;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CourseContentViewModel
            {
                ContentTypes = _courseContentRepository.GetContentTypesAsSelectList()
            };

            var courses = await _courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            return View(viewModel);
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

            viewModel.ContentTypes = _courseContentRepository.GetContentTypesAsSelectList();

            var courses = await _courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title"); 

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);

            viewModel.ContentTypes = _courseContentRepository.GetContentTypesAsSelectList();
            var courses = await _courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title", courseContent.CourseId); // Preselect the current course

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

            viewModel.ContentTypes = _courseContentRepository.GetContentTypesAsSelectList();
            var courses = await _courseRepository.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Title", viewModel.CourseId); // Preselect the current course

            return View(viewModel);
        }

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseContentRepository.DeleteAsync(id);
            return Json(new { success = true });
        }
    }
}