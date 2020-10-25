using System;
using System.Threading.Tasks;
using Discord;
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

            _discord.MessageReceived += OnMessageReceived;
            _commandService.CommandExecuted += OnCommandExecuted;
        }

        private async Task OnMessageReceived(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;

            if (message == null || message.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var argPos = 0;
            
            if (!message.HasStringPrefix(_config["prefix"], ref argPos) ||
                message.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                return;
            }
            
            var context = new SocketCommandContext(_discord, message);
            await _commandService.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result?.Error != null)
            {
                var commandName = command.Value.Name;
                
                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        await context.Channel.SendMessageAsync(
                            "Unknown command! Make sure you're typing the correct command syntax, use `!help` for a list of all commands.");
                        break;
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync(
                            $"Incorrect arguments! You might be using the incorrect amount of arguments or type, use `!help {commandName}` for more information about that command.");
                        break;
                    case CommandError.ObjectNotFound:
                        await context.Channel.SendMessageAsync(
                            $"Not found! If you passed in a user, its likely they don't exist. Use `!help {commandName}` for more info on the command you're trying to use.");
                        break;
                    default:
                        await context.Channel.SendMessageAsync(result.ToString());
                        break;
                }
            }
            
            // TODO: Might be a good idea to add some logging here
        }
    }

    // TODO: May want to separate this out into its own file
    // TODO: Expand on this so it can actually be used
    public class CustomRuntimeResult : RuntimeResult
    {
        public CustomRuntimeResult(CommandError? error, string reason) : base(error, reason)
        {
        }
    }
}