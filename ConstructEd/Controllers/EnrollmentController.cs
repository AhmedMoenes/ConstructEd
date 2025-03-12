using ConstructEd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
       
        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            
        }
    }
}
