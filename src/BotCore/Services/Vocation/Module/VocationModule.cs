using BotCore.Entities;
using BotCore.Extenstions;
using BotCore.Interactions.Preconditions;
using BotCore.Services.VocationForms;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Services.VocationModule
{
    [Group("vocation", "all information about vocation")]
    public class VocationModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UserService _usersService { get; set; }
        public VocationService vocationService { get; set; }
        public SupervisorService supervisorService { get; set; }
        public ChannelService _channelService { get; set; }
        



        [SlashCommand("list", "show all vocation request you did in the past")]
        [ComponentInteraction("vocation-list:*", true)]
        public async Task list(int page = 0)
        {
            await DeferAsync();

            var vocationlist = await vocationService.GetManyAsync(x => x.UserRequestId == Context.BotUser.Id);

            if (vocationlist != null && vocationlist.Count() != 0)
            {
                string times = "";
                string isConfirmed = "";
                string confirmed = "Confirmed";
                string rejected = "Rejected";
                foreach (var vocation in vocationlist)
                {
                    times += $"{vocation.StartDate.ToString("dd / MM / yyyy")} `TO` {vocation.EndDate.ToString("dd / MM / yyyy")} \n";
                    isConfirmed += (vocation.IsAccept ? confirmed : rejected) + "\n";
                }

                var embed = new EmbedBuilder()
                    .AddField("IsConfirmed", isConfirmed, true)
                    .AddField("For date", times, true)
                    .Build();

                var components = new ComponentBuilder()
                    .WithButton("Previous", $"vocation-list:{page - 1}", ButtonStyle.Primary, disabled: page > 0)
                    .WithButton("Next", $"vocation-list:{page + 1}", ButtonStyle.Primary, disabled: vocationlist.Count() > 30)
                    .Build();

                await FollowupAsync(embed: embed, components: components);

                return;
            }
            await FollowupAsync("You didn't request for a day off yet!.");


            // Log the message
            var message = $"Requested for list of the vocations";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);
        }



        [SlashCommand("request", "request for a day off")]
        public async Task request()
        {
            await RespondWithModalAsync<VocationForm>($"vocation-request");
            
        }

        [ModalInteraction("vocation-request", true)]
        public async Task set_modal(VocationForm form)
        {
            await DeferAsync();

            DateTime startDate = form.StartDate;
            DateTime endDate = form.EndDate;

           
            var requestUser = await _usersService.GetAsync(x => x.DiscordId == Context.BotUser.DiscordId);

            //  amount of request for day off
            var vocationDays = Convert.ToInt32((form.EndDate - form.StartDate).TotalDays);

            // The amount of day user can request for Day Off
            var dayOfflimitation =  await vocationService.GetVocationAmountAsync(requestUser.Id);
            if(dayOfflimitation == 0)
            {
                await FollowupAsync("Sorry! You don't have a `Day Off` for this month. Please try next month");
                return;
            }
            if(vocationDays > dayOfflimitation)
            {
                await FollowupAsync($"Sorry! You don't have {vocationDays} days `Day Off` for this month, but you can request for {dayOfflimitation} days");
                return;
            }
            
            //Get employee's supervisor
            var spr = await supervisorService.GetSupervisor(Context.BotUser.Id);

            if(spr == null)
            {
                await FollowupAsync($"Sorry! You don't have a `Supervisor` yet, please contact admin to assign you a `Supervisor`!");
                return;
            }

            // insert
           var vocationId = await vocationService.AddVocationAsync(Context.BotUser.Id, startDate, endDate, spr.SupervisorId, form.Description, Context.BotGuild.Id);

            await FollowupAsync($"Great! You've requested for **{vocationDays}** days vocation for this month, soon I will notify you about your supervisor's answer!");

            var embed = new EmbedBuilder()
            {
                Title = "Vocation Request",
                Description = $"**{Context.BotUser.Username}** wants to a have day off for `{vocationDays}` days \n" +
                $"From Date : `{form.StartDate}` To `{form.EndDate}` \n" +
                $"Please `Confirm` or `Reject` this request so we can notify him/her about your  decision \n\n" +
                $"Description/Reason: {form.Description} \n" +
                $"Request Date : {DateTime.UtcNow.ToString("dddd, dd MMMM yyyy")}"
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Confrim", $"button-vocation-request-confirm:{requestUser.Id}:{vocationId}", ButtonStyle.Success, new Emoji("✔"), null, false, 0)
                .WithButton("Reject", $"button-vocation-request-reject:{requestUser.Id}:{vocationId}", ButtonStyle.Danger, new Emoji("❌"), null, false, 0)
                .Build();

           await this.supervisorService.SendMessageToUserAsync(spr.SupervisorId, "", false, embed: embed, components: component);


            // Log the message
            var message = $"Requested for vocation";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);
        }



        // Reject the vocation Request
        [ComponentInteraction("button-vocation-request-reject:*:*", true)]
        public async Task cancel(int id, int vocationId)
        {
            await DeferAsync();
           
                var spr = new EmbedBuilder()
                {
                    Title = "Request Rejected",
                    Description = $"You've rejected the request for vocation. I just notified (him/her) about your decision! \n ",
                    ThumbnailUrl = "https://img.icons8.com/fluency/480/delete-sign.png",
                    Color = 14946816,
                }.Build();

            var emp = new EmbedBuilder()
            {
                Title = "Request Rejected",
                Description = $"Your request for vocation has rejected due to your supervisor decision! Please contact (him/her) for more info ",
                ThumbnailUrl = "https://img.icons8.com/fluency/480/delete-sign.png",
                Color = 14946816,
            }.Build();

            // Update the database for vocation rejected 
            var user = vocationService.UpdateAsync(vocationId, x =>
            {
                x.IsAccept = false;
            });

            // Notify the user for rejection from supervisor
            await this.supervisorService.SendMessageToUserAsync(id, "", false, embed: emp);

            await FollowupAsync(null, embed: spr);
            
        }

        // Confirm the vocation Request
        [ComponentInteraction("button-vocation-request-confirm:*:*", true)]
        public async Task confirm(int id, int vocationId)
        {
            await DeferAsync();

            var embed = new EmbedBuilder()
            {
                Title = "Request Confirmed!",
                Description = $"You've confirmed the request for vocation. I just notified (him/her) about your decision!",
                ThumbnailUrl = "https://img.icons8.com/fluency/200/verified-account.png",
                Color = 1364764
            }.Build();


            var emp = new EmbedBuilder()
            {
                Title = "Request Confirmed!",
                Description = "**Good News!**  Your request for vocation confirmed 😀 \n  Enjoy your vocation. Good luck!",
                ThumbnailUrl = "https://img.icons8.com/fluency/200/verified-account.png",
                Color = 1364764
            }.Build();

            // Update the database for vocation confirmed 
            var user = vocationService.UpdateAsync(vocationId, x =>
            {
                x.IsAccept = true;
            });

            // Notify the user for rejection from supervisor
            await this.supervisorService.SendMessageToUserAsync(id, "", false, embed: emp);

            await FollowupAsync(null, embed: embed);

        }

    }
}
