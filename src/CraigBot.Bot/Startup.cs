using System;
using System.Threading.Tasks;
using CraigBot.Bot.Services;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using CraigBot.Infrastructure.Repositories;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraigBot.Bot
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json");

            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);

            await startup.RunAsync();
        }

        private async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ILoggingService>();
            provider.GetRequiredService<ICommandHandler>();

            await provider.GetRequiredService<IStartupService>().StartAsync();

            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // TODO: Might be a good idea to create your own implementations of the discord client and command service
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<IStaticDataRepository, StaticDataRepository>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IStartupService, StartupService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);
        }
    }
}