using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    public class ServerController : Controller
    {
        private readonly IBotUserService _botUserService;
        private readonly IBotUserGuildService _botUserGuildService;

        public ServerController(IBotUserService botUserService, IBotUserGuildService botUserGuildService)
        {
            _botUserService = botUserService;
            _botUserGuildService = botUserGuildService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();
            var user = await _botUserService.GetBotUserByDiscordId((ulong)userid);
            var guilds = await _botUserGuildService.GetAllGuildsByUserId(user.Id);
            return View(guilds);
        }
    }
}
