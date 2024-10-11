using Microsoft.AspNetCore.Mvc;

namespace Cls.Ui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
