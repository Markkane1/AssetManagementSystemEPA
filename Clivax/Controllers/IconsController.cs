using Microsoft.AspNetCore.Mvc;

namespace Clivax.Controllers
{
    public class IconsController : Controller
    {
        // GET: Icons
        public IActionResult fontawesome()
        {
            return View();
        }
        public IActionResult materialdesign()
        {
            return View();
        }
    }
}