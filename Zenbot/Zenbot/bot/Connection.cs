﻿using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.bot
{
    public class Connection
    {
        private DiscordSocketClient _client;



        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += ClientOnMessageReceived;

            var token = "MTAxNTk2NzY0Mjc0MzQ4MDQzMw.GjjLsy.csYNkhHAn0v6arJh6g1mNV9U_l7oD716u55gVc";

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private static Task ClientOnMessageReceived(SocketMessage arg)
        {
            if (arg.Content.StartsWith("hello!"))
            {
                arg.Channel.SendMessageAsync($"Hello {arg.Author.Username}!");
            }

            return Task.CompletedTask;
        }
    }
}
