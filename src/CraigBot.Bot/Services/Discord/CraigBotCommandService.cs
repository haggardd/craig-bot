using CraigBot.Core.Services;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Services.Discord
{
    public class CraigBotCommandService : CommandService
    {
        private static readonly CommandServiceConfig Configuration = new CommandServiceConfig
        {
            LogLevel = LogSeverity.Verbose,
            // TODO: Check if this is needed and visa versa
            DefaultRunMode = RunMode.Async 
        };
        
        public CraigBotCommandService(ILoggingService loggingService) : base(Configuration)
        {
            Log += loggingService.OnLog;
        }
    }
}