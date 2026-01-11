using Microsoft.AspNetCore.Mvc;

namespace Clivax.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public IActionResult login()
        {
            return View();
        }
        public IActionResult register()
        {
            return View();
        }
    }
}