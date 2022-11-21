using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]
    public class JoinServerController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IWelcomeMessageService _welcomeMessageService;
        private readonly INotyfService _notyf;

        public JoinServerController(IBotUserGuildService botUserGuildService, IWelcomeMessageService welcomeMessageService, INotyfService notyf)
        {
            _botUserGuildService = botUserGuildService;
            _welcomeMessageService = welcomeMessageService;
            _notyf = notyf;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();
            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }


        public async Task<IActionResult> Edit(int guildId, string guildName)
        {
            var message = await _welcomeMessageService.GetWelcomeMessagesByGuildId(guildId);

            ViewBag.guildId = guildId;
            ViewBag.guildName = guildName;

            if (message != null)
            {
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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WelcomeMessageDto dto)
        {
            var bMessage = await _welcomeMessageService.UpdateWelcomeMessage(dto);
            if (bMessage) {
                _notyf.Success("Welcome message updated successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notyf.Error("Error occured during updating Welcome message"); return View(dto);
            }
        }
    }
}
