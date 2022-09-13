using Zenbot.ActionAttributes;

using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Zenbot.Configuration;
using Zenbot.Data;
using Zenbot.ActionModules;

namespace Zenbot.Services
{
    public class ActionHandlingService
    {
        private readonly IOptions<BirthdayConfiguration> _birthdayConfig;
        private readonly RestService _myRest;
        private readonly DiscordSocketClient _client;
        private readonly TimerFactory _timerFactory;
        private readonly IBirthdaysRepository _birthdays;

        private readonly IServiceProvider _services; // Perhaps later I can get rid of this atrocity, but for now I just want the modules thing to work

        private List<(Timer timer, MethodInfo action)> _actions;


        public ActionHandlingService(IOptions<BirthdayConfiguration> birthdayConfig, RestService myRest, DiscordSocketClient client, TimerFactory timerFactory, IBirthdaysRepository birthdays, IServiceProvider services)
        {
            Console.WriteLine("Action Handler initialising...");

            _birthdayConfig = birthdayConfig;
            _myRest = myRest;
            _client = client;
            _timerFactory = timerFactory;
            _birthdays = birthdays;

            _services = services;
        }

        /// <summary>
        /// Loads Actions into the list and initialises their designated timers
        /// </summary>
        /// <remarks>
        /// Timers will only start once ActionHandlingService is initialised,
        /// <br/>and since instantiating is happening within DI container - 
        /// <br/>a method for manual initialisation is required.
        /// </remarks>
        public async Task InitializeAsync()
        {
            await AddActionsAsync();
        }

        /// <summary>
        /// Finds Actions via Reflection, and initialises their designated timers
        /// </summary>
        private async Task AddActionsAsync()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            _actions = assembly.DefinedTypes
                // All public types derived from IActionModule
                .Where(typeInfo => typeInfo.IsPublic && typeof(IActionModule).GetTypeInfo().IsAssignableFrom(typeInfo))
                // All methods declared directly in these types, and not inherited; to exclude common methods such as GetType or Equals
                .Select(typeInfo => typeInfo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList())
                .SelectMany(methodInfo => methodInfo)
                .Select(methodInfo => (GetTimer(methodInfo), methodInfo))
                .ToList();

            await RunStartupActions();
        }

        /// <summary>
        /// Runs actions at startup if they have RunAtStartup attribute
        /// </summary>
        private async Task RunStartupActions()
        {
            var startupActions = _actions
                .Select(record => record.action)
                .Where(action => RunAtStartup(action))
                .ToList();

            foreach (var action in startupActions)
            {
                await RunMethod(action);
            }
        }

        /// <summary>
        /// Executes action method via reflection
        /// </summary>
        private async Task RunMethod(MethodInfo method)
        {
            var type = method.DeclaringType;

            // Split this query into parts for ease of comprehension
            var typeConstructor = type.GetConstructors().First();
            var typeConstructorArgs = typeConstructor.GetParameters().Select(parameterInfo => parameterInfo.ParameterType);
            var typeConstructorArgsArray = typeConstructorArgs.Select(arg => _services.GetRequiredService(arg)).ToArray();

            var moduleInstance = typeConstructor.Invoke(typeConstructorArgsArray);

            await (Task)method.Invoke(moduleInstance, null);
        }

        /// <summary>
        /// Generates Timer for a repeatable action, and assigns action to run every time the timer elapses
        /// </summary>
        private Timer GetTimer(MethodInfo method)
        {
            Timer timer = _timerFactory.CreateTimer(TimerIntervalInMilliseconds(method));

            timer.Elapsed += async (object sender, ElapsedEventArgs e) => await RunMethod(method);
            timer.Start();

            return timer;
        }

        private int TimerIntervalInMilliseconds(MethodInfo method) =>
            method.GetCustomAttribute<TimerAttribute>().IntervalInMilliseconds;

        private bool RunAtStartup(MethodInfo method) =>
            !(method.GetCustomAttribute<RunAtStartupAttribute>() is null);
    }
}
