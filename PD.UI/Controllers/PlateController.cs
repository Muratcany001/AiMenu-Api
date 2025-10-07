using Microsoft.AspNetCore.Mvc;

namespace PD.UI.Controllers
{
    public class PlateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
