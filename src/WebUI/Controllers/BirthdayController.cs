using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]
    public class BirthdayController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IBirthdayMessageService _birthdayMessageService;

        public BirthdayController(IBotUserGuildService botUserGuildService, IBirthdayMessageService birthdayMessageService)
        {
            _botUserGuildService = botUserGuildService;
            _birthdayMessageService = birthdayMessageService;
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
            var message = await _birthdayMessageService.GetBirthdayMessagesByGuildId(guildId);

            ViewBag.guildId = guildId;
            ViewBag.guildName = guildName;

            if (message != null)
            {
                var messageDto = new BirthdayMessageDto
                {
                    Message = message.Message,
                    IsActive = message.IsActive,
                    GuildId = message.GuildId,
                    Id = message.Id
                };
                return View(messageDto);
            }
            ViewBag.info = "The placeholder is the default Message for Birthday, but you can always customize it.";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BirthdayMessageDto dto)
        {
            var bMessage = await _birthdayMessageService.UpdateBirthdayMessage(dto);
            if (bMessage) return RedirectToAction(nameof(Index));
            else return View(dto);
        }
    }
}
