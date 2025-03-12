using ConstructEd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class CourseContentController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        public CourseContentController(ICourseContentRepository courseContentRepository,
          ICourseRepository courseRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
        }
    }
}
