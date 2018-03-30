using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using CalibotCS.Services.Log;

namespace CalibotCS
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        
        private DiscordSocketClient m_client;
        private IConfiguration m_config;
        
        //Main Async Function
        public async Task MainAsync()
        {
            //Create the client
            m_client = new DiscordSocketClient();
            //Build the config file
            m_config = BuildConfig();
            var serviceProvider = ConfigureServices();
            serviceProvider.GetRequiredService<LogService>();

            await serviceProvider.GetRequiredService<CommandHandlingService>().InitializeAsync(serviceProvider);

            await m_client.LoginAsync(TokenType.Bot, m_config["token"]);
            await m_client.StartAsync();
            await m_client.SetGameAsync("with Calipao");

            //Don't let the bot end
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(m_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                // Logging
                .AddLogging()
                .AddSingleton<LogService>()
                // Extra
                .AddSingleton(m_config)
                //Finally build it
                .BuildServiceProvider();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}
