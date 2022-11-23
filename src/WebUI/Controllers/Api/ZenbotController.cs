using BotCore.Interactions.Shared;
using BotCore.Services;
using BotCore.Services.Bitbucket;
using BotCore.Services.Jira;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.BitbucketWebHook;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.JiraWebHook;


namespace Zenbot.WebUI.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ZenbotController : ZenController
    {
        private readonly IServiceProvider _services;
        private readonly JiraService _jiraService;
        private readonly BitbucketService _bitbucketService;
        private readonly ChannelService _channelService;
        public ZenbotController(IServiceProvider services, JiraService jiraService)
        {
            _services = services;
            _jiraService = jiraService;
            _bitbucketService = services.GetRequiredService<BitbucketService>();
            _channelService = services.GetRequiredService<ChannelService>();
        }



        // For Jira Webhook
        [HttpPost]
        public async Task JiraWebHook([FromBody] JsonElement entity)
        {
            try
            {
                    var json = entity.GetRawText();
                    var result = JsonConvert.DeserializeObject<JiraWebhookObject>(json);

                    var jiraWH = new JiraWebHookDetail
                    {
                        AssigneeId = result.issue.fields.assignee.accountId,
                        IssueSelf = result.issue.self,
                        IssueName = result.issue.fields.summary,
                        ProjectName = result.issue.fields.project.name,
                        ProjectUrl = result.issue.fields.project.self,
                        ReporterName = result.issue.fields.reporter.displayName,
                        IssueUpdateDate = result.issue.fields.statuscategorychangedate
                    };
                    var embed = new EmbedBuilder()
                    {
                        Title = "New Task Assigned",
                        Description = $"**{jiraWH.ReporterName}** has assigned you a new task \n" +
                        $"{jiraWH.IssueName} : {jiraWH.IssueSelf} \n" +
                        $"Date : {jiraWH.IssueUpdateDate.ToString("dddd, dd MMMM yyyy")}"
                    }.Build();

                    var component = new ComponentBuilder()

                        .WithButton("Project Name", "2", ButtonStyle.Secondary, null, "", true, row: 0)
                        .WithButton(jiraWH.ProjectName, null, ButtonStyle.Link, null, jiraWH.ProjectUrl, row: 0)

                        .Build();

                    await this._jiraService.SendMessageToUserAsync(jiraWH.AssigneeId, "", false, embed: embed, components: component);
            }
            catch (Exception ex)
            {
                var message = "An exception occured during receiving Jira webhook: "+ ex.Message;
                await _channelService.loggerEmbedMessage(message);
            }
            
        }


        // For Bitbucket Webhook

        [HttpPost]
        public async Task BitbucketWebHook([FromBody] BitbucketWebHookRequest value)
        {
            try
            {
                if (value != null)
                {
                    var reviewers = value.pullrequest.reviewers.ToArray();

                    var bitbucketWH = new BitbucketWebHook
                    {
                        AuthorName = value.pullrequest.author.display_name,
                        PullRequestDate = value.pullrequest.created_on,
                        PullRequestLink = value.pullrequest.links.html.href,
                        PullRequestTitle = value.pullrequest.title,
                        RepositoryName = value.repository.name,
                        RepositoryLink = value.repository.links.html.href,
                        PullRequestId = value.pullrequest.id,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Title = "New Pull Request",
                        Description = $"**{bitbucketWH.AuthorName}**  added you as a reviewer on pull request #{bitbucketWH.PullRequestId} \n" +
                        $"Link: {bitbucketWH.PullRequestLink} \n Date : {bitbucketWH.PullRequestDate.ToString("dddd, dd MMMM yyyy")}"
                    }.Build();

                    var component = new ComponentBuilder()
                        .WithButton("Author", "1", ButtonStyle.Secondary, null, null, true, row: 0)
                        .WithButton(SharedButtonModule.GetButtonReturnLabelName(bitbucketWH.AuthorName, ButtonStyle.Secondary), row: 0)

                        .WithButton("Repository", "3", ButtonStyle.Secondary, null, "", true, row: 1)
                        .WithButton(bitbucketWH.RepositoryName, null, ButtonStyle.Link, null, bitbucketWH.RepositoryLink, row: 1)


                        .Build();

                    foreach (var item in reviewers)
                    {
                        await this._bitbucketService.SendMessageToUserAsync(item.account_id, "", false, embed: embed, components: component);
                        await Task.Delay(100);
                    }
                }
                else
                {
                    var message = "Could not get the bitBucket webhook data! \n Value of bitBucket webhook is null";
                    await _channelService.loggerEmbedMessage(message);
                }
            }
            catch (Exception ex)
            {
                var message = "An exception occured during receiving bitBucket webhook: " + ex.Message;
                await _channelService.loggerEmbedMessage(message);
            }
          

        }

    }
}
