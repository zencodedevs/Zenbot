using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    public class JoinServerController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IWelcomeMessageService _welcomeMessageService;

        public JoinServerController(IBotUserGuildService botUserGuildService, IWelcomeMessageService welcomeMessageService)
        {
            _botUserGuildService = botUserGuildService;
            _welcomeMessageService = welcomeMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();
            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }


        public async Task<IActionResult> Edit(int guildId)
        {
            var message = await _welcomeMessageService.GetWelcomeMessagesByGuildId(guildId);
            if (message != null)
            {
                ViewBag.guildId = guildId;
                var messageDto = new WelcomeMessageDto
                {
                    Message = message.Message,
                    IsActive = message.IsActive,
                    GuildId = message.GuildId,
                    Id = message.Id
                };
                return View(messageDto);
            }
            ViewBag.info = "The placeholder is the default Message for Welcomeing the new user, but you can always customize it.";
            ViewBag.guildId = guildId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WelcomeMessageDto dto)
        {
            var bMessage = await _welcomeMessageService.UpdateWelcomeMessage(dto);
            if (bMessage) return RedirectToAction(nameof(Index));
            else return View(dto);
        }
    }
}
