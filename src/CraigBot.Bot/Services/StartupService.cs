using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Modules;
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
        private readonly BotOptions _botOptions;
        private readonly ModuleFlagOptions _moduleFlags;

        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commandService,
            IOptions<BotOptions> botOptions,
            IOptions<ModuleFlagOptions> moduleFlags)
        {
            _provider = provider;
            _discord = discord;
            _commandService = commandService;
            _botOptions = botOptions.Value;
            _moduleFlags = moduleFlags.Value;
        }

        public async Task StartClient()
        {
            if (string.IsNullOrWhiteSpace(_botOptions.Token))
            {
                throw new Exception("Your token is missing or configured incorrectly.");
            }

            await _discord.LoginAsync(TokenType.Bot, _botOptions.Token);
            await _discord.StartAsync();
            await _discord.SetActivityAsync(new Game($"{_botOptions.Prefix}help", ActivityType.Listening));
            
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            
            await CheckModuleFlags();

            if (!_commandService.Modules.Any())
            {
                throw new Exception("There are no modules enabled.");
            }
        }

        private async Task CheckModuleFlags()
        {
            if (!_moduleFlags.Audio)
            {
                await _commandService.RemoveModuleAsync<AudioModule>();
            }
            
            if (!_moduleFlags.Banking)
            {
                await _commandService.RemoveModuleAsync<BankingModule>();
            }
            
            if (!_moduleFlags.Betting)
            {
                await _commandService.RemoveModuleAsync<BettingModule>();
            }
            
            if (!_moduleFlags.Fun)
            {
                await _commandService.RemoveModuleAsync<FunModule>();
            }
            
            if (!_moduleFlags.Help)
            {
                await _commandService.RemoveModuleAsync<HelpModule>();
            }
            
            if (!_moduleFlags.Miscellaneous)
            {
                await _commandService.RemoveModuleAsync<MiscellaneousModule>();
            }
            
            if (!_moduleFlags.Moderation)
            {
                await _commandService.RemoveModuleAsync<ModerationModule>();
            }
            
            if (!_moduleFlags.Poll)
            {
                await _commandService.RemoveModuleAsync<PollModule>();
            }
            
            if (!_moduleFlags.Utility)
            {
                await _commandService.RemoveModuleAsync<UtilityModule>();
            }
        }
    }
}