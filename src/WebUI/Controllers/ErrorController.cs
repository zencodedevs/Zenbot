using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zenbot.WebUI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: /Error/Code500
        [HttpGet]
        public IActionResult Code500()
        {
            return View();
        }

        // GET: /Error/Code404
        [HttpGet]
        public IActionResult Code404()
        {
            return View();
        }

        // GET: /Error/Code403
        [HttpGet]
        public IActionResult Code403()
        {
            return View();
        }
    }
}
