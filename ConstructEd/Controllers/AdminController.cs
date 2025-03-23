using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IPluginRepository _pluginRepository;
        private readonly IStatisticsRepository _statisticsRepository; 
        private readonly IContactFormRepository _contactFormRepository;
        private readonly IMapper _mapper;

        #region Constructor
        public AdminController(IAuthService authService,
                               ICourseRepository courseRepository,
                               IInstructorRepository instructorRepository,
                               ICourseContentRepository courseContentRepository,
                               IPluginRepository pluginRepository,
                               IStatisticsRepository statisticsRepository,
                               IContactFormRepository contactFormRepository,
                               IMapper mapper)

        {
                                _authService = authService;
                                _courseRepository = courseRepository;
                                _instructorRepository = instructorRepository;
                                _courseContentRepository = courseContentRepository;
                                _pluginRepository = pluginRepository;
                                _statisticsRepository = statisticsRepository; 
                                _contactFormRepository = contactFormRepository;
                                _mapper = mapper;
        }
        #endregion

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var statistics = await _statisticsRepository.GetStatisticsAsync();
            var messages = await _contactFormRepository.GetAllContactFormsAsync();
            ViewBag.Statistics = statistics;
            ViewBag.Messages = messages;
            return View(nameof(Dashboard));
        }
        #endregion

        #region Courses Management
        public async Task<IActionResult> CourseIndex()
        {
            var courses = await _courseRepository.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<CourseViewModel>>(courses); 
            return View(nameof(CourseIndex), viewModels); 
        }
        public async Task<IActionResult> CreateCourse()
        {
            var viewModel = new CourseViewModel();
            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();
            return View(nameof(CreateCourse), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CourseViewModel viewModel)
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
                    return RedirectToAction(nameof(Dashboard));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();
            return View(nameof(CreateCourse), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseViewModel>(course);

            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();
            return View(nameof(EditCourse), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, CourseViewModel viewModel)
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
                    return RedirectToAction(nameof(CourseIndex));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            ViewBag.instructorList = await _instructorRepository.GetAllAsync();
            ViewBag.categoryList = _courseRepository.GetCategories();

            return View(nameof(EditCourse), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseViewModel>(course);
            return View(nameof(RemoveCourse), viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCourseConfirmed(int id)
        {
            try
            {
                await _courseRepository.DeleteAsync(id);
                return RedirectToAction(nameof(CourseIndex));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(RemoveCourse), await _courseRepository.GetByIdAsync(id));
            }
        }
        #endregion

        #region CourseContents Management
        public async Task<IActionResult> CourseContentIndex()
        {
            var courseContents = await _courseContentRepository.GetAllAsync();
            var viewModels = _mapper.Map<List<CourseContentViewModel>>(courseContents);
            var courses = await _courseContentRepository.GetCourseNamesAsync();
            ViewBag.Courses = courses;
            return View(viewModels);
        }
         public async Task<IActionResult> CourseContentDetails(int id)
        {
            var courseContents = await _courseContentRepository.GetCourseContent(id);
            var viewModels = _mapper.Map<List<CourseContentViewModel>>(courseContents);
            ViewBag.CourseId = id;
            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCourseContent(int id)
        {
            var viewModel = new CourseContentViewModel
            {
                CourseId = id
            };

            ViewBag.ContentTypes = _courseContentRepository.GetCategories();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourseContent(CourseContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseContent = _mapper.Map<CourseContent>(viewModel);
                await _courseContentRepository.InsertAsync(courseContent);
                await _courseContentRepository.SaveAsync();
                return RedirectToAction(nameof(CourseIndex));
            }

            ViewBag.ContentTypes = _courseContentRepository.GetCategories();

            return View(viewModel);
        }

       

        [HttpGet]
        public async Task<IActionResult> EditCourseContent(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);
            ViewBag.ContentTypes = _courseContentRepository.GetCategories();

            return View(nameof(EditCourseContent),viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourseContent(CourseContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var courseContent = _mapper.Map<CourseContent>(viewModel);
                await _courseContentRepository.UpdateAsync(courseContent);
                return RedirectToAction(nameof(CourseContentIndex));
            }
            ViewBag.ContentTypes = _courseContentRepository.GetCategories();

            return View(nameof(EditCourseContent),viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCourseContent(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);
            return View(nameof(RemoveCourseContent), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCourseContentConfirmed(int id)
        {
            try
            {
                await _courseContentRepository.DeleteAsync(id);
                return RedirectToAction(nameof(CourseContentDetails));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(RemoveCourseContent), await _courseContentRepository.GetByIdAsync(id));
            }
        }

        #endregion

        #region Plugins Management
        public async Task<IActionResult> PluginIndex()
        {
            var plugins = await _pluginRepository.GetAllAsync();
            var viewModel = _mapper.Map<IEnumerable<PluginViewModel>>(plugins);
            return View(nameof(PluginIndex), viewModel);
        }
        [HttpGet]
        public IActionResult CreatePlugin()
        {
            var viewModel = new PluginViewModel();
            return View(nameof(CreatePlugin), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlugin(PluginViewModel viewModel)
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

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    viewModel.Image = fileName;
                }
                try
                {
                    var plugin = _mapper.Map<Plugin>(viewModel);
                    await _pluginRepository.InsertAsync(plugin);
                    return RedirectToAction(nameof(PluginIndex));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }
            return View(nameof(CreatePlugin), viewModel);
        }

        public async Task<IActionResult> EditPlugin(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<PluginViewModel>(plugin);

            return View(nameof(EditPlugin), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlugin(int id, PluginViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var plugin = _mapper.Map<Plugin>(viewModel);
                    await _pluginRepository.UpdateAsync(plugin);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(nameof(EditPlugin), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemovePlugin(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            return View(nameof(RemovePlugin), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            try
            {
                await _pluginRepository.DeleteAsync(id);
                return RedirectToAction(nameof(PluginIndex));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(RemoveConfirmed), await _pluginRepository.GetByIdAsync(id));
            }
        }
        #endregion

        #region Instructors Management
        [HttpGet]
        public async Task<IActionResult> InstructorIndex()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return View(nameof(InstructorIndex), instructors);
        }
        public IActionResult CreateInstructor()
        {
            return View(nameof(CreateInstructor));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstructor(InstructorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(viewModel);
                user.IsInstructor = true;

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    // Define the folder to save the image
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    // Generate a unique file name
                    var fileName = viewModel.Email;
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
                    return RedirectToAction("Dashboard", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(nameof(CreateInstructor), viewModel);
        }

        public async Task<IActionResult> EditInstructor(string id)
        {

            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<InstructorViewModel>(instructor);

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

            return View(nameof(EditInstructor), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstructor(string id, InstructorViewModel viewModel)
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
                try
                {
                    var instructor = _mapper.Map<ApplicationUser>(viewModel);
                    await _instructorRepository.UpdateAsync(instructor);
                    return RedirectToAction(nameof(InstructorIndex));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            var instructors = await _instructorRepository.GetAllAsync();
            return View(nameof(EditInstructor), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveInstructor(string id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<InstructorViewModel>(instructor);
            return View(nameof(RemoveInstructor), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveInstructorConfirmed(string id)
        {
            try
            {
                await _instructorRepository.DeleteAsync(id);
                return RedirectToAction(nameof(InstructorIndex));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                var instructor = await _instructorRepository.GetByIdAsync(id);
                var viewModel = _mapper.Map<InstructorViewModel>(instructor);
                return View(nameof(RemoveInstructor), viewModel);
            }
        }

        #endregion

       

    }
}
