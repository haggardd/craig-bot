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
            
            services.AddDbContext<CraigBotDbContext>();

            services.AddHttpClient<IImageService, ImageService>();
            
            services.AddSingleton<DiscordSocketClient, CraigBotClient>()
                .AddScoped<CommandService, CraigBotCommandService>()
                .AddScoped<IFortuneRepository, FortuneRepository>()
                .AddScoped<IAskResponseRepository, AskResponseRepository>()
                .AddScoped<IBankAccountRepository, BankAccountRepository>()
                .AddScoped<IBetRepository, BetRepository>()
                .AddScoped<IWagerRepository, WagerRepository>()
                .AddScoped<IStockRepository, StockRepository>()
                .AddScoped<IInvestmentRepository, InvestmentRepository>()
                .AddScoped<ICommandHandler, CommandHandler>()
                .AddScoped<IStartupService, StartupService>()
                .AddScoped<IPollService, PollService>()
                .AddScoped<IBankingService, BankingService>()
                .AddScoped<IBettingService, BettingService>()
                .AddScoped<IInvestingService, InvestingService>()
                .AddScoped<ILoggingService, LoggingService>()
                .AddScoped<Random>()
                .AddSingleton(Configuration);

            services.AddOptions()
                .Configure<BotOptions>(Configuration.GetSection(BotOptions.ConfigurationHeader))
                .Configure<ApiOptions>(Configuration.GetSection(ApiOptions.ConfigurationHeader))
                .Configure<ModuleFlagOptions>(Configuration.GetSection(ModuleFlagOptions.ConfigurationHeader));

            return services;
        }
    }
}