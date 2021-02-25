using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Common;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    [Summary("Banking Commands")]
    public class BankingModule : CraigBotBaseModule
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
            var account = await _bankingService.GetOrCreateAccount(Context.User);

            var embed = BaseBankingEmbed()
                .WithTitle("Bank Statement")
                .AddField("Balance", $"`{_options.Currency}{account.Balance:N2}`");

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("wire")]
        [Summary("Wires a given amount of funds to another user.")]
        [Example("wire 10 @Craig")]
        [Example("wire 1.10 @Craig")]
        [Example("wire .01 @Craig")]
        [PreventSelfMention]
        public async Task Wire([Summary("The amount of funds you wish to send.")] decimal amount, 
            [Summary("The user you wish to send funds to.")] SocketGuildUser user)
        {
            if (BankingHelpers.IsBelowMinimum(amount))
            {
                await MentionReply($"The minimum amount you can send is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`.", ResponseTypes.Information);
                return;
            }
            
            var payerAccount = await _bankingService.GetOrCreateAccount(Context.User);
            var payeeAccount = await _bankingService.GetOrCreateAccount(user);

            if (!payerAccount.CanAfford(amount))
            {
                await MentionReply("You don't have enough funds to make that transaction.", ResponseTypes.Information);
                return;
            }

            await _bankingService.Withdraw(payerAccount, amount);
            await _bankingService.Deposit(payeeAccount, amount);

            await ReplyAsync($"Transaction successful! {Context.User.Mention} sent `{_options.Currency}{amount:N2}` to {user.Mention}.");
        }
        
        [Command("grant")]
        [Summary("Grants a user or yourself with free funds from the magic bank.")]
        [Example("grant 10 @Craig")]
        [Example("grant 1.10 @Craig")]
        [Example("grant .01 @Craig")]
        [Example("grant 1000.00")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Grant([Summary("The amount of funds you wish to grant.")] decimal amount, 
            [Summary("The user you wish to receive the grant.")] SocketGuildUser user = null)
        {
            if (BankingHelpers.IsBelowMinimum(amount))
            {
                await MentionReply($"The minimum amount you can grant is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`.", ResponseTypes.Information);
                return;
            }
            
            var account = await _bankingService.GetOrCreateAccount(user ?? Context.User);

            await _bankingService.Deposit(account, amount);
            
            // TODO: Should probably change these to use the MentionReply method
            await ReplyAsync($"Grant successful! {(user ?? Context.User).Mention} has been granted `{_options.Currency}{amount:N2}`.");
        }
        
        [Command("fine")]
        [Summary("Fines a user.")]
        [Example("fine 1.10 @Craig")]
        [Example("fine .01 @Craig")]
        [Example("fine 1000.00 @Craig")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Fine([Summary("The size of the fine.")] decimal amount, 
            [Summary("The user you wish to fine.")] SocketGuildUser user)
        {
            if (BankingHelpers.IsBelowMinimum(amount))
            {
                await MentionReply($"The minimum amount you can fine is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`.", ResponseTypes.Information);
                return;
            }
            
            var account = await _bankingService.GetOrCreateAccount(user);
            
            if (!account.CanAfford(amount))
            {
                await MentionReply("That user can't afford to pay a fine of that size.", ResponseTypes.Information);
                return;
            }

            await _bankingService.Withdraw(account, amount);
            
            await ReplyAsync($"Fine successful! {user.Mention} has been fined `{_options.Currency}{amount:N2}`.");
        }
        
        #endregion
        
        #region Helpers

        private EmbedBuilder BaseBankingEmbed()
            => new EmbedBuilder()
                .WithColor(Color.Green)
                .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl());

        #endregion
    }
}