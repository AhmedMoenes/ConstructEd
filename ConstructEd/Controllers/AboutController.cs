using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
