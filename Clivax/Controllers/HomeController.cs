using Microsoft.AspNetCore.Mvc;

namespace Clivax.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public IActionResult Index()
        {
            return View();
        }
    }
}