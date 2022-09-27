using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.BotCore;

namespace Zenbot.WebUI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {
        private readonly DiscordBotService _botService;
        public ZenbotController(DiscordBotService botService)
        {
            this._botService = botService;
        }

 
        [HttpGet]
        public async Task GetJiraWebhook(string id)
        {
            _botService.
        }

    }
}
