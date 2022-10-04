using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.BotCore;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.JiraWebHook;

namespace Zenbot.WebUI.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ZenbotController : ZenController
    {
        private readonly EventService _eventservice;
       
        public ZenbotController(EventService eventService)
        {
            _eventservice = eventService;
        }

        [HttpGet]
        public async Task GetJiraWebhook(string id)
        {
            await _eventservice.SendMessageToUserByJiraId(id);
        }

        [HttpPost]
        public async Task GetJiraWebHook([FromBody] JiraWebhookObject value)
        {
            
        }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
   

}
