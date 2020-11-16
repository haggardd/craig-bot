using CraigBot.Core.Services;
using Discord;
using Discord.WebSocket;

namespace CraigBot.Bot.Services.Discord
{
    public class CraigClient : DiscordSocketClient
    {
        private static readonly DiscordSocketConfig Configuration = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            MessageCacheSize = 1000,
            AlwaysDownloadUsers = true
        };
        
        public CraigClient(ILoggingService loggingService) : base(Configuration)
        {
            Log += loggingService.OnLog;
        }
    }
}