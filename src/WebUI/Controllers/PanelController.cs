using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zenbot.WebUI.Controllers
{
    //[Authorize]
    public class PanelController : Controller
    {
        // GET: /Panel/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
