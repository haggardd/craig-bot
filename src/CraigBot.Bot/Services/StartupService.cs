using System;
using System.Reflection;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Services
{
    public class StartupService : IStartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        private readonly BotOptions _options;

        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commandService,
            IOptions<BotOptions> options)
        {
            _provider = provider;
            _discord = discord;
            _commandService = commandService;
            _options = options.Value;
        }

        public async Task StartClient()
        {
            if (string.IsNullOrWhiteSpace(_options.Token))
            {
                throw new Exception("Your token is missing or configured incorrectly.");
            }

            await _discord.LoginAsync(TokenType.Bot, _options.Token);
            await _discord.StartAsync();
            await _discord.SetActivityAsync(new Game($"{_options.Prefix}help", ActivityType.Listening));
            
            // TODO: Figure a way to determine if the module is disabled, use the new flags in the options
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}