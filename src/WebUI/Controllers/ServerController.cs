using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    public class ServerController : Controller
    {
       
        private readonly IBotUserGuildService _botUserGuildService;

        public ServerController(IBotUserGuildService botUserGuildService)
        {
            _botUserGuildService = botUserGuildService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();
            
            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }
    }
}
