using Microsoft.AspNetCore.Mvc;

namespace PD.UI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
