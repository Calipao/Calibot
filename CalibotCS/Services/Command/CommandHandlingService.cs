using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace CalibotCS
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient m_discordClient;
        private readonly CommandService m_commandService;
        private readonly IConfiguration m_config;
        private IServiceProvider m_serviceProvider;

        public CommandHandlingService(IServiceProvider _serviceProvider, DiscordSocketClient _discordClient, CommandService _commandService, IConfiguration _configuration)
        {
            m_discordClient = _discordClient;
            m_commandService = _commandService;
            m_serviceProvider = _serviceProvider;
            m_config = _configuration;

            m_discordClient.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider _serviceProvider)
        {
            m_serviceProvider = _serviceProvider;
            await m_commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            int argPos = 0;
            if (!message.HasStringPrefix(m_config["prefix"], ref argPos)) return;

            var context = new SocketCommandContext(m_discordClient, message);
            var result = await m_commandService.ExecuteAsync(context, argPos, m_serviceProvider);

            if (result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync(result.ToString());
        }
    }
}