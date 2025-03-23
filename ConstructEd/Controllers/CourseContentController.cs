using AutoMapper;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            ;
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);

            ViewBag.CourseName = courseContent.Course?.Title;
            ViewBag.CourseID = courseContent.Course?.Id;

            return View(viewModel);
        }

    }
}