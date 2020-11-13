using CraigBot.Core.Services;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Services.Discord
{
    public class CraigCommandService : CommandService
    {
        private static readonly CommandServiceConfig Configuration = new CommandServiceConfig
        {
            LogLevel = LogSeverity.Verbose,
            DefaultRunMode = RunMode.Async // TODO: Check if this is needed and visa versa
        };
        
        public CraigCommandService(ILoggingService loggingService) : base(Configuration)
        {
            Log += loggingService.OnLog;
        }
    }
}