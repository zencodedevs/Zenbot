using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Zenbot.bot;

namespace Zenbot
{
    public class Program
    {
       
        public static void Main(string[] args) => new Program().connection.MainAsync().GetAwaiter().GetResult();

        Connection connection = new Connection();
    }

}

