using Api;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Cls.Ui.Controllers
{
    public class DoctorCardsController : Controller
    {

            private ICrudService _api;

        public DoctorCardsController(ICrudService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _api.GetAllAsync<Doctor>("Doctor/GetAllDoctors");
            return View(res);
        }
    }
}
