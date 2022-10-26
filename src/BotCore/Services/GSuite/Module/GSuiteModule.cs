using BotCore.Entities;
using BotCore.Services.GSuite.Form;
using BotCore.Services.ScrinIO;
using Discord;
using Discord.Interactions;
using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.GSuite.Module
{
    [Group("gsuite", "G Suite commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]

    [RequireBotPermission(GuildPermission.ManageRoles)]
    [RequireContext(ContextType.Guild)]
    public class GSuiteModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public GsuiteServices gsuiteServices { get; set; }
        public ChannelService _channelService { get; set; }
     

        [SlashCommand("create-account", "Create a new G suite account for a user")]
        public async Task add()
        {
            await RespondWithModalAsync<GSuiteForm>($"set-gsuite-account");
        }

        [ModalInteraction("set-gsuite-account", true)]
        public async Task set_modal(GSuiteForm form)
        {
            await DeferAsync();

            // check for correct email address
            if (!form.Email.Contains('@'))
            {
                await FollowupAsync("Please enter the correct email address with your domain name");
                return;
            }

            // check for correct email address
            if (form.Email.Contains("@gmail.com"))
            {
                await FollowupAsync("Domain not found! Please enter a primary email with your domain nam");
                return;
            }


            // Get data and insert to service
            User newUserbody = new User();
            UserName newUsername = new UserName();

            newUsername.GivenName = form.FirstName;
            newUsername.FamilyName = form.Family;

            newUserbody.PrimaryEmail = form.Email;
            newUserbody.Name = newUsername;
            newUserbody.Password = form.Password;

            var Credentials = Context.BotGuild.GSuiteAuth;
            var result =  gsuiteServices.CreateGSuiteAccount(newUserbody, Credentials);

           await FollowupAsync($"A G Suite Account created as **{form.Email}** successfully!");

            // Log the message
            var message = $"Requested to create a g suite accout for a user `{form.Email}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }
    }
}
