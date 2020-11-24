using System;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Models;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
            var account = await _bankingService.GetAccount(Context.User);

            var embed = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithTitle("Bank Statement")
                .WithTimestamp(DateTimeOffset.Now)
                .AddField("Balance", $"{_options.Currency}{account.Balance:0.00}")
                .WithFooter(f =>
                {
                    f.Text = Context.Message.Author.Username;
                    f.IconUrl = Context.Message.Author.GetAvatarUrl();
                });

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("wire")]
        [Summary("Wires a given amount of funds to another user.")]
        public async Task Wire([Summary("The amount of funds you wish to send.")] decimal amount, 
            [Summary("The user you wish to send funds to.")] SocketGuildUser user)
        {
            var payerAccount = await _bankingService.GetAccount(Context.User);
            var payeeAccount = await _bankingService.GetAccount(user);

            if (Context.User.Id == user.Id)
            {
                await ReplyAsync("You can't send funds to yourself!");
                return;
            }

            if (!CanAffordWithdrawal(payerAccount, amount))
            {
                await ReplyAsync("You don't have enough funds to make that transaction!");
                return;
            }

            if (amount < 0.01M)
            {
                await ReplyAsync($"The minimum amount you can send it {_options.Currency}0.01!");
                return;
            }

            var roundedAmount = Math.Round(amount, 2);

            await _bankingService.WithdrawFromAccount(payerAccount, roundedAmount);
            await _bankingService.DepositToAccount(payeeAccount, roundedAmount);

            await ReplyAsync($"Transaction successful! {Context.User.Mention} sent {_options.Currency}{roundedAmount:0.00} to {user.Mention}");
        }
        
        #endregion

        #region Helpers

        private bool CanAffordWithdrawal(BankAccount account, decimal amount)
            => account.Balance - amount > 0;

        #endregion
    }
}