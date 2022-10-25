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
     

        [SlashCommand("create-account", "Create a new G suite account for a user")]
        public async Task add()
        {
            await RespondWithModalAsync<GSuiteForm>($"set-gsuite-account");
        }

        [ModalInteraction("set-gsuite-account", true)]
        public async Task set_modal(GSuiteForm form)
        {
            await DeferAsync();

            User newUserbody = new User();
            UserName newUsername = new UserName();

            newUsername.GivenName = form.FirstName;
            newUsername.FamilyName = form.Family;

            newUserbody.PrimaryEmail = form.Email;
            newUserbody.Name = newUsername;
            newUserbody.Password = form.Password;

            var Credentials = "credential.json";
            var result =  gsuiteServices.CreateGSuiteAccount(newUserbody, Credentials);

           await FollowupAsync($"A G Suite Account created as **{form.Email}** successfully!");
        }
    }
}
