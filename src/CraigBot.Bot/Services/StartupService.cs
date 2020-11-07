using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        private readonly IConfigurationRoot _config;

        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commandService,
            IConfigurationRoot config)
        {
            _provider = provider;
            _discord = discord;
            _commandService = commandService;
            _config = config;
        }

        public async Task StartAsync()
        {
            var discordToken = _config["Token"];
            var prefix = _config["Settings:Prefix"];

            if (string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception("Your token is missing or configured incorrectly.");
            }

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();
            await _discord.SetActivityAsync(new Game($"{prefix}help", ActivityType.Listening));

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}