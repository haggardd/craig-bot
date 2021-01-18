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

        private Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", false, false);

            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup();

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

            services.AddHttpClient<IImageService, ImageService>();
            
            services.AddSingleton<DiscordSocketClient, CraigBotClient>()
                .AddSingleton<CommandService, CraigBotCommandService>()
                .AddTransient<IFortuneCookieRepository, FortuneCookieRepository>()
                .AddTransient<IEightBallResponseRepository, EightBallResponseRepository>()
                .AddTransient<IBankAccountRepository, BankAccountRepository>()
                .AddTransient<IBetRepository, BetRepository>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<IStartupService, StartupService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddSingleton<IPollService, PollService>()
                .AddTransient<IBankingService, BankingService>()
                .AddTransient<IBetService, BetService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<Random>()
                .AddSingleton(Configuration);

            services.AddOptions()
                .Configure<BotOptions>(Configuration.GetSection(BotOptions.ConfigurationHeader))
                .Configure<ApiOptions>(Configuration.GetSection(ApiOptions.ConfigurationHeader))
                .Configure<ModuleFlagOptions>(Configuration.GetSection(ModuleFlagOptions.ConfigurationHeader));

            return services;
        }
    }
}