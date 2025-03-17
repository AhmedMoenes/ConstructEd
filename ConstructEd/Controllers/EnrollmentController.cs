using AutoMapper;

using ConstructEd.Models;

using ConstructEd.Repositories;

using ConstructEd.ViewModels;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using System.Threading.Tasks;

public class EnrollmentsController : Controller

{

    private readonly IEnrollmentRepository _enrollmentRepository;

    private readonly IMapper _mapper;

    public EnrollmentsController(IEnrollmentRepository enrollmentRepository, IMapper mapper)

    {

        _enrollmentRepository = enrollmentRepository;

        _mapper = mapper;

    }

    // Enroll in a course

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Enroll(int courseId)

    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)

        {

            return Unauthorized("User not authenticated.");

        }

        // Check if the user is already enrolled

        var existingEnrollment = await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);

        if (existingEnrollment != null)

        {

            return BadRequest("You are already enrolled in this course.");

        }

        var enrollment = new Enrollment

        {

            UserId = userId,

            CourseId = courseId,

            EnrollmentDate = DateTime.UtcNow,

            Progress = 0

        };

        await _enrollmentRepository.InsertAsync(enrollment);

        await _enrollmentRepository.SaveAsync();

        return RedirectToAction(nameof(MyCourses));

    }

    // Get the user's enrolled courses

    [HttpGet]

    public async Task<IActionResult> MyCourses()

    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)

        {

            return Unauthorized("User not authenticated.");

        }

        var enrollments = await _enrollmentRepository.GetByUserIdAsync(userId);

        var enrollmentViewModels = _mapper.Map<List<EnrollmentViewModel>>(enrollments);

        return View(enrollmentViewModels);

    }

}

