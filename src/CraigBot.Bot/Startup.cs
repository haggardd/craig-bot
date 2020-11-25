using System;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Services;
using CraigBot.Bot.Services.Discord;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using CraigBot.Infrastructure.Database;
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
            var services = ConfigureServices();

            var provider = services.BuildServiceProvider();

            var dbContext = provider.GetRequiredService<CraigBotDbContext>();
            await DbInitialiser.Initialise(dbContext);
            
            provider.GetRequiredService<ILoggingService>();
            provider.GetRequiredService<ICommandHandler>();
            
            await provider.GetRequiredService<IStartupService>().StartClient();

            await Task.Delay(-1);
        }

        private ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            
            // TODO: Still need to look into the lifetime of the context, not sure it's correct
            services.AddDbContext<CraigBotDbContext>(ServiceLifetime.Transient);
            
            // TODO: If the custom command service and client don't get used, might as well go back to using the old ones
            services.AddSingleton<DiscordSocketClient, CraigClient>()
                .AddSingleton<CommandService, CraigCommandService>()
                .AddTransient<IFortuneCookieRepository, FortuneCookieRepository>()
                .AddTransient<IEightBallResponseRepository, EightBallResponseRepository>()
                .AddTransient<IBankAccountRepository, BankAccountRepository>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IStartupService, StartupService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddTransient<IBankingService, BankingService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);

            services.AddOptions()
                .Configure<BotOptions>(Configuration.GetSection(BotOptions.ConfigurationHeader))
                .Configure<ModuleFlagOptions>(Configuration.GetSection(ModuleFlagOptions.ConfigurationHeader));

            return services;
        }
    }
}