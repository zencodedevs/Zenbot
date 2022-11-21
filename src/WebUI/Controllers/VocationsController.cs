using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]
    public class VocationsController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IVocationServices _vocationServices;

        public VocationsController(IBotUserGuildService botUserGuildService, IVocationServices vocationServices)
        {
            _botUserGuildService = botUserGuildService;
            _vocationServices = vocationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();

            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }


        public async Task<IActionResult> VocationList(int guildId, string guildName)
        {
            ViewBag.guildName = guildName;
            var vocations = await _vocationServices.GetVocationListByGuildId(guildId);
            return View(vocations);
        }
    }
}
