﻿// Originally made by jalaljaleh for Dynastio.Discord
//Read the original file at https://github.com/jalaljaleh/Dynastio.Discord/blob/master/Dynastio.Bot/Interactions/Preconditions/RateLimit.cs
//Originally licensed under Apache 2.0 https://github.com/jalaljaleh/Dynastio.Discord/blob/master/LICENSE.txt
//Modifications:
// 
namespace Discord.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    using System.Collections.Concurrent;

    public class RateLimit : PreconditionAttribute
    {
        private static ConcurrentDictionary<ulong, List<RateLimitItem>> Items = new ConcurrentDictionary<ulong, List<RateLimitItem>>();
        private static DateTime _removeExpiredCommandsTime = DateTime.MinValue;
        private readonly RateLimitType? _context;
        private readonly RateLimitBaseType _baseType;
        private readonly int _requests;
        private readonly int _seconds;
        public RateLimit(int seconds = 4, int requests = 1, RateLimitType context = RateLimitType.User, RateLimitBaseType baseType = RateLimitBaseType.BaseOnCommandInfo)
        {
            this._context = context;
            this._requests = requests;
            this._seconds = seconds;
            this._baseType = baseType;
        }
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            // clear old expired commands every 30m
            if (DateTime.UtcNow > _removeExpiredCommandsTime)
            {
                _ = Task.Run(async () =>
                {
                    await ClearExpiredCommands();
                    _removeExpiredCommandsTime = DateTime.UtcNow.AddMinutes(30);
                });
            }

            ulong id = _context.Value switch
            {
                RateLimitType.User => context.User.Id,
                RateLimitType.Channel => context.Channel.Id,
                RateLimitType.Guild => context.Guild.Id,
                RateLimitType.Global => 0,
                _ => 0
            };

            var contextId = _baseType switch
            {
                RateLimitBaseType.BaseOnCommandInfo => commandInfo.Module.Name + "//" + commandInfo.Name + "//" + commandInfo.MethodName,
                RateLimitBaseType.BaseOnCommandInfoHashCode => commandInfo.GetHashCode().ToString(),
                RateLimitBaseType.BaseOnSlashCommandName => (context.Interaction as SocketSlashCommand).CommandName,
                RateLimitBaseType.BaseOnMessageComponentCustomId => (context.Interaction as SocketMessageComponent).Data.CustomId,
                RateLimitBaseType.BaseOnAutocompleteCommandName => (context.Interaction as SocketAutocompleteInteraction).Data.CommandName,
                RateLimitBaseType.BaseOnApplicationCommandName => (context.Interaction as SocketApplicationCommand).Name,
                _ => "unknown"
            };

            var dateTime = DateTime.UtcNow;

            var target = Items.GetOrAdd(id, new List<RateLimitItem>());

            var commands = target.Where(
                a =>
                a.command == contextId
            );

            foreach (var c in commands.ToList())
                if (dateTime >= c.expireAt)
                    target.Remove(c);

            if (commands.Count() < _requests)
            {
                target.Add(new RateLimitItem()
                {
                    command = contextId,
                    expireAt = DateTime.UtcNow + TimeSpan.FromSeconds(_seconds)
                });
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError($"This command is usable <t:{((DateTimeOffset)target.Last().expireAt).ToUnixTimeSeconds()}:R>."));
        }
        public static Task ClearExpiredCommands()
        {
            foreach (var doc in Items)
            {
                var utcTime = DateTime.UtcNow;
                foreach (var command in doc.Value.Where(a => utcTime > a.expireAt).ToList())
                    doc.Value.Remove(command);
            }
            return Task.CompletedTask;
        }
        private class RateLimitItem
        {
            public string command { get; set; }
            public DateTime expireAt { get; set; }
        }
        public enum RateLimitType
        {
            User,
            Channel,
            Guild,
            Global
        }
        public enum RateLimitBaseType
        {
            BaseOnCommandInfo,
            BaseOnCommandInfoHashCode,
            BaseOnSlashCommandName,
            BaseOnMessageComponentCustomId,
            BaseOnAutocompleteCommandName,
            BaseOnApplicationCommandName,
            BaseOnApplicationCommandId,
        }
    }
}