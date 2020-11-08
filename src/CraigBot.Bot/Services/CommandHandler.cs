using System;
using System.Threading.Tasks;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Services
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;
        private readonly string _prefix;
        
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

            _prefix = _config["Settings:Prefix"];
            
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
            
            if (!message.HasStringPrefix(_prefix, ref argPos) ||
                message.HasMentionPrefix(_discord.CurrentUser, ref argPos))
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
                    $"Unknown command! Make sure you're typing the correct command syntax, use `{_prefix}help` for a list of all commands.",
                CommandError.BadArgCount => 
                    $"Incorrect arguments! You might be using the incorrect amount of arguments or type, use `{_prefix}help {command.Value.Name}` for more information about that command.",
                CommandError.ObjectNotFound => 
                    $"Not found! If you passed in a user, its likely they don't exist. Use `{_prefix}help {command.Value.Name}` for more info on the command you're trying to use.",
                CommandError.UnmetPrecondition => 
                    "Unmet precondition! You lack the correct precondition, this is most likely because you lack the correct permissions to use this command.",
                CommandError.MultipleMatches =>
                    $"Multiple matches! Multiple matches were found for `{_prefix}{command.Value.Name}`, use `{_prefix}help {command.Value.Name}` for info on specific differences.",
                CommandError.ParseFailed =>
                    $"Parsing failure! `{_prefix}{command.Value.Name}` failed to parse, this has been logged.`",
                CommandError.Unsuccessful =>
                    $"Unsuccessful! `{_prefix}{command.Value.Name}` failed to execute successfully, this has been logged.",
                CommandError.Exception => 
                    $"Exception thrown! `{_prefix}{command.Value.Name}` threw an exception, this has been logged.",
                _ => result.ToString()
            };

            await context.Channel.SendMessageAsync(message);
        }
    }
}