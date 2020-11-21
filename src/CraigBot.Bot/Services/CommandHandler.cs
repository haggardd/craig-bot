using System;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Services
{
    // TODO: Add more preconditions for bot and user permissions
    // TODO: Look into where commands should be preformed, i.e. DMs and channel messages
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        private readonly BotOptions _options;
        private readonly IServiceProvider _provider;

        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commandService,
            IOptions<BotOptions> options,
            IServiceProvider provider)
        {
            _discord = discord;
            _commandService = commandService;
            _options = options.Value;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceived;
            _commandService.CommandExecuted += OnCommandExecuted;
        }

        public async Task OnMessageReceived(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;

            if (message == null || message.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var argPos = 0;
            
            if (!message.HasStringPrefix(_options.Prefix, ref argPos))
            {
                return;
            }
            
            var context = new SocketCommandContext(_discord, message);
            
            await _commandService.ExecuteAsync(context, argPos, _provider);
        }

        public async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.Error == null)
            {
                return;
            }

            var message = result.Error switch
            {
                CommandError.UnknownCommand => 
                    $"Unknown command! Make sure you're typing the correct command syntax, use `{_options.Prefix}help` for a list of all commands.",
                CommandError.BadArgCount => 
                    $"Incorrect arguments! You might be using the incorrect amount of arguments or type, use `{_options.Prefix}help {command.Value.Name}` for more information about that command.",
                CommandError.ObjectNotFound => 
                    $"Not found! If you passed in a user, its likely they don't exist. Use `{_options.Prefix}help {command.Value.Name}` for more info on the command you're trying to use.",
                CommandError.UnmetPrecondition => 
                    "Unmet precondition! You lack the correct precondition, this is most likely because you lack the correct permissions to use this command.",
                CommandError.MultipleMatches =>
                    $"Multiple matches! Multiple matches were found for `{_options.Prefix}{command.Value.Name}`, use `{_options.Prefix}help {command.Value.Name}` for info on specific differences.",
                CommandError.ParseFailed =>
                    $"Parsing failure! `{_options.Prefix}{command.Value.Name}` failed to parse, this has been logged.",
                CommandError.Unsuccessful =>
                    $"Unsuccessful! `{_options.Prefix}{command.Value.Name}` failed to execute successfully, this has been logged.",
                CommandError.Exception => 
                    $"Exception thrown! `{_options.Prefix}{command.Value.Name}` threw an exception, this has been logged.",
                _ => result.ToString()
            };

            await context.Channel.SendMessageAsync(message);
        }
    }
}