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

        public async Task OnMessageReceived(IMessage message)
        {
            if (!(message is SocketUserMessage userMessage) 
                || userMessage.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }

            var argPos = 0;
            
            if (!userMessage.HasStringPrefix(_options.Prefix, ref argPos))
            {
                return;
            }
            
            var context = new SocketCommandContext(_discord, userMessage);
            
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
                    $"Unmet precondition! Reason: `{await command.Value.CheckPreconditionsAsync(context)}`.",
                CommandError.MultipleMatches =>
                    $"Multiple matches! Multiple matches were found for `{_options.Prefix}{command.Value.Name}`, use `{_options.Prefix}help {command.Value.Name}` for info on specific differences.",
                // TODO: Could add the failed parse result here
                CommandError.ParseFailed =>
                    $"Parsing failure! `{_options.Prefix}{command.Value.Name}` failed to parse, you likely used an incorrect parameter type. Use `{_options.Prefix}help {command.Value.Name}` for more info.",
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