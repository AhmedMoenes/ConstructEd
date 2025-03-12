using ConstructEd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseContentRepository courseContentRepository,
          ICourseRepository courseRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
        }
    }
}
