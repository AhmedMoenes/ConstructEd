using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class PluginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
