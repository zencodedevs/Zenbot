using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]
    public class ServerController : Controller
    {
       
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IGuildService _guildService;

        public ServerController(IBotUserGuildService botUserGuildService, IGuildService guildService)
        {
            _botUserGuildService = botUserGuildService;
            _guildService = guildService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();
            
            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthenticateGuild(int guildId, string password)
        {
            await _guildService.UpdatePasswordForGuild(guildId, password);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GSuiteAuth(int guildId, IFormFile gsuite)
        {
            await _guildService.UpdateGSuiteAuthForGuild(guildId, gsuite);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScrinIOToken(int guildId, string scrinio)
        {
            await _guildService.UpdateScrinIOForGuild(guildId, scrinio);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
