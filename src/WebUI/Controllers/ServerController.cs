using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;
        public ServerController(IBotUserGuildService botUserGuildService, IGuildService guildService, INotyfService notyf)
        {
            _botUserGuildService = botUserGuildService;
            _guildService = guildService;
            _notyf = notyf;
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
            var result =await _guildService.UpdatePasswordForGuild(guildId, password);
            if (result)
            {
                _notyf.Success("Password for guild updated successfully");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            _notyf.Error("Error occured during updating password");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GSuiteAuth(int guildId, IFormFile gsuite)
        {
            var result = await _guildService.UpdateGSuiteAuthForGuild(guildId, gsuite);
            if (result)
            {
                _notyf.Success("G suite credentials updated successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyf.Error("Error occured during saving G suite file");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScrinIOToken(int guildId, string scrinio)
        {
            var result = await _guildService.UpdateScrinIOForGuild(guildId, scrinio);
            if (result)
            {
                _notyf.Success("Scrin_io token updated successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyf.Error("Error occured during updating guild");
            return RedirectToAction(nameof(Index));
        }
    }
}
