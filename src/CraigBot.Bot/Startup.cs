﻿using System;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Services;
using CraigBot.Bot.Services.Discord;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using CraigBot.Infrastructure.Repositories;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraigBot.Bot
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", false, false);

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

            await provider.GetRequiredService<IStartupService>().StartClient();

            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DiscordSocketClient, CraigClient>()
                .AddSingleton<CommandService, CraigCommandService>()
                .AddSingleton<IStaticDataRepository, StaticDataRepository>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IStartupService, StartupService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);

            services.AddOptions()
                .Configure<BotOptions>(Configuration.GetSection(BotOptions.ConfigurationHeader))
                .Configure<ModuleFlagOptions>(Configuration.GetSection(ModuleFlagOptions.ConfigurationHeader));
        }
    }
}