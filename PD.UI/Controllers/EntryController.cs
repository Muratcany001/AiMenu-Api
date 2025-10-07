using Microsoft.AspNetCore.Mvc;

namespace PD.UI.Controllers
{
    public class EntryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
