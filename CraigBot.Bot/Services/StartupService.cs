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
        // TODO: Don't like the name '_commands', change to '_commandService'
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;
        }

        public async Task StartAsync()
        {
            var discordToken = _config["tokens:discord"];

            if (string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception("Your token is missing or configured incorrectly.");
            }

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();
            await _discord.SetActivityAsync(new Game($"{_config["prefix"]}help", ActivityType.Listening));
            
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}