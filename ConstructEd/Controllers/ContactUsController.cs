using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
