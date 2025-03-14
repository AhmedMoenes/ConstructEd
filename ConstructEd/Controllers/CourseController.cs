using ConstructEd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly IPaymentRepository paymentRepository;

        public CourseController(ICourseContentRepository courseContentRepository,
          ICourseRepository courseRepository,IEnrollmentRepository enrollmentRepository,IPaymentRepository paymentRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.paymentRepository = paymentRepository;
        }
    }
}
