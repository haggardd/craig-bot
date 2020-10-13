using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commandService,
            IConfigurationRoot config,
            IServiceProvider provider)
        {
            _discord = discord;
            _commandService = commandService;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;

            if (message == null || message.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var context = new SocketCommandContext(_discord, message);
            var argPos = 0;

            if (message.HasStringPrefix(_config["prefix"], ref argPos) ||
                message.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commandService.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)
                {
                    // TODO: Bad code! Take a look at the docs!
                    // https://discord.foxbot.me/docs/guides/commands/post-execution.html
                    switch (result.Error)
                    {
                        case CommandError.UnknownCommand:
                            await context.Channel.SendMessageAsync(
                                "Unknown command! Make sure you're typing the correct command syntax, use '!help' for a list of all commands.");
                            break;
                        case CommandError.BadArgCount:
                            await context.Channel.SendMessageAsync(
                                "Incorrect arguments! You might be using the incorrect amount of arguments, use '!help' for a list of all commands and their arguments.");
                            break;
                        default:
                            await context.Channel.SendMessageAsync(result.ToString());
                            break;
                    }
                }
            }
        }
    }
}