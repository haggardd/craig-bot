using System;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    [Summary("Banking Commands")]
    [RequireContext(ContextType.Guild)]
    public class BankingModule : ModuleBase<SocketCommandContext>
    {
        private readonly IBankingService _bankingService;
        private readonly BotOptions _options;
        
        public BankingModule(IBankingService bankingService, IOptions<BotOptions> options)
        {
            _bankingService = bankingService;
            _options = options.Value;
        }
        
        #region Commands
        
        [Command("bank")]
        [Summary("Provides the user with a bank statement.")]
        public async Task Bank()
        {
            var bank = await _bankingService.GetAccount(Context.User);

            var embed = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithTitle("Bank Statement")
                .WithTimestamp(DateTimeOffset.Now)
                .AddField("Balance", $"{_options.Currency}{bank.Balance:0.00}")
                .WithFooter(f =>
                {
                    f.Text = Context.Message.Author.Username;
                    f.IconUrl = Context.Message.Author.GetAvatarUrl();
                });

            await ReplyAsync("", false, embed.Build());
        }
        
        #endregion
    }
}