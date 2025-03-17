using AutoMapper;

using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

public class EnrollmentController : Controller

{

    private readonly IEnrollmentRepository _enrollmentRepository;

    private readonly IMapper _mapper;

    public EnrollmentController(IEnrollmentRepository enrollmentRepository, IMapper mapper)

    {

        _enrollmentRepository = enrollmentRepository;

        _mapper = mapper;

    }
  
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

        // Fetch only paid enrollments
        var enrollments = await _enrollmentRepository.GetAllEnrollmentsByUserIdAsync(userId);

        var viewModel = new EnrollmentViewModel
        {
            UserName = User.Identity.Name,
            Courses = enrollments
                .Where(e => e.Course != null)
                .Select(e => new CourseViewModel
                {
                    Id = e.Course.Id,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    Duration = e.Course.Duration,
                    Category = e.Course.Category,
                    EnrolledDate=e.Course.Enrollments.FirstOrDefault().EnrollmentDate
                }).ToList(),
            Plugins = enrollments
                .Where(e => e.Plugin != null)
                .Select(e => new PluginViewModel
                {
                    Id = e.Plugin.Id,
                    Title = e.Plugin.Title,
                    Description = e.Plugin.Description,
                    EnrolledDate = e.Plugin.Enrollments.FirstOrDefault().EnrollmentDate

                }).ToList()
        };

        return View(viewModel);
    }

}

