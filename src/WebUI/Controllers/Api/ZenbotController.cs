using BotCore.Services.Jira;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {
        private readonly EventService _eventservice;
        private readonly Data _jiraData;
        public ZenbotController(EventService eventService, Data jiraData)
        {
            _eventservice = eventService;
            _jiraData = jiraData;
        }

        [HttpGet]
        public async Task GetJiraWebhook(string id)
        {
            await _eventservice.SendMessageToUserByJiraId(id);
        }

        [HttpPost]
        public async Task GetJiraWebHook([FromBody] JiraWebhookObject value)
        {
            var user = await _jiraData.GetBotUserWithJiraAccount(value.issue.fields.assignee.accountId);
            if (user is not null)
            {
                var jiraWH = new JiraWebHook
                {
                    AssigneeId = value.issue.fields.assignee.accountId,
                    AssigneeName = value.issue.fields.assignee.displayName,
                    IssueSelf = value.issue.self,
                    PriorityIconUrl = value.issue.fields.priority.iconUrl,
                    PriorityName = value.issue.fields.priority.name,
                    ProjectName = value.issue.fields.project.name,
                    ProjectUrl = value.issue.fields.project.self,
                    ReporterName = value.issue.fields.reporter.displayName
                };
            }
        }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
   

}
