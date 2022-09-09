using System;
using Zenbot.BotConnection;

namespace Zenbot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().connection.MainAsync().GetAwaiter().GetResult();

        Connection connection = new Connection();
    }
}
