﻿using System;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Models;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    [Summary("Banking Commands")]
    [RequireContext(ContextType.Guild)]
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
            var account = await _bankingService.GetAccountOrCreateAccount(Context.User);

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
        [Example("wire 10 @Craig")]
        [Example("wire 1.10 @Craig")]
        [Example("wire .01 @Craig")]
        public async Task Wire([Summary("The amount of funds you wish to send.")] decimal amount, 
            [Summary("The user you wish to send funds to.")] SocketGuildUser user)
        {
            if (Context.User.Id == user.Id)
            {
                await ReplyAsync("You can't send funds to yourself!");
                return;
            }
            
            if (amount < MinimumAmount)
            {
                await ReplyAsync($"The minimum amount you can send is `{_options.Currency}{MinimumAmount}`!");
                return;
            }
            
            var payerAccount = await _bankingService.GetAccountOrCreateAccount(Context.User);
            var payeeAccount = await _bankingService.GetAccountOrCreateAccount(user);

            if (!CanAffordWithdrawal(payerAccount, amount))
            {
                await ReplyAsync("You don't have enough funds to make that transaction!");
                return;
            }

            var roundedAmount = Math.Round(amount, 2);

            await _bankingService.Withdraw(payerAccount, roundedAmount);
            await _bankingService.Deposit(payeeAccount, roundedAmount);

            await ReplyAsync($"Transaction successful! {Context.User.Mention} sent `{_options.Currency}{roundedAmount:0.00}` to {user.Mention}.");
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
            if (amount < MinimumAmount)
            {
                await ReplyAsync($"The minimum amount you can grant is `{_options.Currency}{MinimumAmount}`!");
                return;
            }
            
            var account = await _bankingService.GetAccountOrCreateAccount(user ?? Context.User);
            
            var roundedAmount = Math.Round(amount, 2);
            
            await _bankingService.Deposit(account, roundedAmount);
            
            await ReplyAsync($"Grant successful! {(user ?? Context.User).Mention} has been granted `{_options.Currency}{roundedAmount:0.00}`!");
        }
        
        #endregion

        // TODO: Might be a good idea to separate these into a helper / extensions class
        #region Helpers

        private const decimal MinimumAmount = 0.01M;
        
        private bool CanAffordWithdrawal(BankAccount account, decimal amount)
            => account.Balance - amount > 0;

        #endregion
    }
}