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

        // testing if data comes to our bot we shoul check if we have this user in discord then send message to 
        [HttpPost]
        [Route(nameof(CreateJira))]
        public Task CreateJira(string jiraId)
        {
            _botService.Event(jiraId);

            return Task.CompletedTask;
        }

    }
}
