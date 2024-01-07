using Microsoft.AspNetCore.Mvc;

namespace Agency.Areas.Admin.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
