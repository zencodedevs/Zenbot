using BotCore.Extenstions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class SupervisorService : EntityBaseService<SupervisorEmployee>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        private readonly UserService usersService;

        public SupervisorService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            usersService = services.GetService<UserService>();
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        public async Task<IMessage> SendMessageToUserAsync(int sprId, string text = null, bool mentionUser = false, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            var user = await usersService.GetUserById(sprId);
            if (user is not null)
            {
                return await usersService.SendMessageToUserAsync(user.DiscordId, mentionUser ? user.ToUserMention().PadRight(1) : "" + text, isTTS, embed, options, allowedMentions, components);
            }
            return null;
        }


        public async Task<SupervisorEmployee> GetOrAddAsync(int supervisor, int user)
        {
            var newMessage = new SupervisorEmployee()
            {
               EmployeeId = user,
               SupervisorId = supervisor,
            };
            var spr = await base.GetAsync(x=> x.EmployeeId == user && x.SupervisorId == supervisor);
            if(spr == null)
            {
                var result = await base.InsertAsync(newMessage);
                return result;
            }
            return null;

        }

        public async Task<SupervisorEmployee> GetSupervisor(int requestId)
        {
            return await base.GetAsync(x => x.EmployeeId == requestId);
        }
    }
}
