using Microsoft.AspNetCore.Mvc;

namespace Cls.Ui.Controllers
{
    public class RegesterLoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
