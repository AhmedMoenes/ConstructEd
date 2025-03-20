using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
