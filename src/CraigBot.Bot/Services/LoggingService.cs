using System;
using System.IO;
using System.Threading.Tasks;
using CraigBot.Core.Services;
using Discord;

namespace CraigBot.Bot.Services
{
    // TODO: Need more logging for non-Discord.net related stuff and check what is already logged
    public class LoggingService : ILoggingService
    {
        private string LogDirectory { get; }
        private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");

        public LoggingService()
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        }

        public Task OnLog(LogMessage message)
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