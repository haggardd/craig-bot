using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commandService;
        
        private string LogDirectory { get; }
        private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");

        public LoggingService(DiscordSocketClient discord, CommandService commandService)
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            _discord = discord;
            _commandService = commandService;

            _discord.Log += OnLog;
            _commandService.Log += OnLog;
        }

        private Task OnLog(LogMessage message)
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            if (!File.Exists(LogFile))
            {
                File.Create(LogFile).Dispose();
            }
            
            var logText = 
                $"{DateTime.UtcNow:hh:mm:ss} [{message.Severity}] {message.Source}: {message.Exception?.ToString() ?? message.Message}";
            
            File.AppendAllText(LogFile, logText + "\n");

            return Console.Out.WriteLineAsync(logText);
        }
    }
}