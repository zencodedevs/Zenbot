using Microsoft.AspNetCore.Mvc;

namespace Zenbot.WebUI.Controllers
{
    public class ServerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
