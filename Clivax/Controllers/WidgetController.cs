using Microsoft.AspNetCore.Mvc;

namespace Clivax.Controllers
{
    public class WidgetController : Controller
    {
        // GET: Widget
        public IActionResult chart()
        {
            return View();
        }
        public IActionResult generals()
        {
            return View();
        }
    }
}