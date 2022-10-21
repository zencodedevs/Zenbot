using BotCore.Entities;
using BotCore.Services.GSuite.Form;
using BotCore.Services.ScrinIO;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.GSuite.Module
{
    [Group("gsuite", "G Suite commands")]
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

            Name name = new Name
            {
                GivenName = form.FirstName,
                FamilyName = form.Family
            };

            GSuiteAccount gSuite = new GSuiteAccount
            {
                Name = name,
                Password = form.Password,
                PrimaryEmail = form.Email
            };

            
           var result = await gsuiteServices.CreateGSuiteAccount(gSuite);

           await FollowupAsync(result);
        }
    }
}
