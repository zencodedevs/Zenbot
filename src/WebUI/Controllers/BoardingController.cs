using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Common;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]

    public class BoardingController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IBoardingMessage _boardingMessageService;
        private readonly IBoardingFiles _boardingFiles;
        private readonly INotyfService _notyf;

        public BoardingController(IBotUserGuildService botUserGuildService, IBoardingMessage boardingMessageService, IBoardingFiles boardingFiles, INotyfService notyf)
        {
            _botUserGuildService = botUserGuildService;
            _boardingMessageService = boardingMessageService;
            _boardingFiles = boardingFiles;
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
            var message = await _boardingMessageService.GetBoardingMessagesByGuildId(guildId);

            ViewBag.guildId = guildId;
            ViewBag.guildName = guildName;


            if (message != null)
            {
                ICollection<BoardingFiles> messageFile = await _boardingFiles.GetBoardingFilesByBoardingMessageId(message.Id);
                ViewBag.messageFiles = messageFile;
                var messageDto = new BoardingMessageDto
                {
                    Message = message.Message,
                    IsActive = message.IsActive,
                    GuildId = message.GuildId,
                    Id = message.Id
                };
                ViewBag.messageId = messageDto.Id;
                return View(messageDto);
            }
            ViewBag.info = "The placeholder is the default Message for Birthday, but you can always customize it.";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BoardingMessageDto dto)
        {
            var bMessage = await _boardingMessageService.UpdateBoardingMessage(dto);
            if (bMessage)
            {
                _notyf.Success("Boarding message updated successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notyf.Error("Error occured during updating Boarding message"); return View(dto);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachFiles(List<IFormFile> files, int messageId, int guildId, string guildName)
        {
            await _boardingFiles.UpdateBoardingFiles(files, messageId, guildId);
            return RedirectToAction("Edit", new { guildId = guildId, guildName = guildName });
        }
    }
}
