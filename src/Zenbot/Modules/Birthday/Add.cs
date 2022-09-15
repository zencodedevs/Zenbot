using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Shared.Entities.Zenbot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;

namespace Zenbot.Modules.Birthday
{
    public class Add : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Add(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        // Uses to add/update birthdays to record.
        [Command("add")]
        public async Task BdayAsync()
        {
            var register_btn = new ButtonBuilder()
            {
                Label = "Register to Zenbot",
                CustomId = "register_btn",
                Style = ButtonStyle.Primary
            };

            //var User = Context.User as SocketGuildUser;

            //using (var scope = _scopeFactory.CreateScope())
            //{
            //    var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            //    var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();
            //    using (var uow = unitOfWorkManager.Begin())
            //    {
            //        var botUser = new BotUser().Create(User.Username, mail, (User.Id).ToString(), Month, Day);

            //        await _repository.InsertAsync(botUser);
            //        await _repository.SaveChangesAsync(true);
            //    }
            //}

            var component = new ComponentBuilder();
            component.WithButton(register_btn);
            await ReplyAsync("Perfect! I won't forget it!", components: component.Build());
        }


        [ComponentInteraction("register_btn")]
        public async Task ModalInfoInput()
        {
            await RespondWithModalAsync<BotInfoModal>("userInfo_modal");
        }

        [ModalInteraction("userInfo_modal")]
        public async Task ModalHandlerInput(BotInfoModal modal)
        {
            string name = modal.Name;
            string email = modal.EmailAddress;
            await RespondAsync(name + email);
        }
    }


    public class BotInfoModal: IModal
    {
        public string Title => "Your information for Zenbot";
        [InputLabel("Your Name")]
        [ModalTextInput("username", TextInputStyle.Short, placeholder: "Your Name for Zenbot", minLength: 5)]

        public string title => "something";
        [InputLabel("Your Email")]
        [ModalTextInput("user_mail", TextInputStyle.Short, placeholder: "Your Email for Zenbot", minLength: 10)]

       public string Name { get; set; }

       public string EmailAddress { get; set; }
    }
}
