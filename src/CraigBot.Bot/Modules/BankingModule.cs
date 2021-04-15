using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
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
        private readonly IInvestingService _investingService;
        private readonly BotOptions _options;
        
        public BankingModule(IBankingService bankingService, IInvestingService investingService, 
            IOptions<BotOptions> options)
        {
            _bankingService = bankingService;
            _investingService = investingService;
            _options = options.Value;
        }
        
        #region Commands
        
        // TODO: This could be expanded upon
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
                await InlineReply(Context.Message, $"The minimum amount you can send is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`");
                return;
            }
            
            var payerAccount = await _bankingService.GetOrCreateAccount(Context.User);
            var payeeAccount = await _bankingService.GetOrCreateAccount(user);

            if (!payerAccount.CanAfford(amount))
            {
                await InlineReply(Context.Message, "You don't have enough funds to make that transaction");
                return;
            }

            await _bankingService.Withdraw(payerAccount, amount);
            await _bankingService.Deposit(payeeAccount, amount);

            await ReplyAsync($"Transaction successful! {Context.User.Mention} sent `{_options.Currency}{amount:N2}` to {user.Mention}");
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
                await InlineReply(Context.Message, $"The minimum amount you can grant is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`");
                return;
            }
            
            var account = await _bankingService.GetOrCreateAccount(user ?? Context.User);

            await _bankingService.Deposit(account, amount);
            
            // TODO: Should probably change these to use the InlineReply method
            await ReplyAsync($"Grant successful! {(user ?? Context.User).Mention} has been granted `{_options.Currency}{amount:N2}`");
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
                await InlineReply(Context.Message, $"The minimum amount you can fine is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`");
                return;
            }
            
            var account = await _bankingService.GetOrCreateAccount(user);
            
            if (!account.CanAfford(amount))
            {
                await InlineReply(Context.Message, "That user can't afford to pay a fine of that size");
                return;
            }

            await _bankingService.Withdraw(account, amount);
            
            await ReplyAsync($"Fine successful! {user.Mention} has been fined `{_options.Currency}{amount:N2}`");
        }
        
        [Command("stocks")]
        [Summary("View all stocks.")]
        public async Task Stocks()
        {
            var stocks = await _investingService.GetAllStocks();

            var embed = BaseBankingEmbed()
                .WithTitle("Stock Market")
                .WithDescription($"The market updates every `{_options.MarketUpdateRate}` minutes. Use `{_options.Prefix}invest` to buy stocks.");

            var stocksText = stocks.Aggregate("", (current, stock) 
                => current + $"• {stock.ToFormattedString(_options.Currency)}\n");

            embed.AddField("Stocks", stocksText);

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("buy")]
        [Summary("Buy a given amount of a specific stock at its current price.")]
        [Example("buy TES 10")]
        [Example("buy POS 2")]
        public async Task Buy([Summary("The stock you wish to buy.")] string stockTicker, 
            [Summary("The amount you wish to buy.")] int amount)
        {
            stockTicker = stockTicker.ToUpper();
            
            if (amount <= 0)
            {
                await InlineReply(Context.Message, $"The minimum amount of stock you can buy is `1`");
                return;
            }
            
            var stock = await _investingService.GetStockByTicker(stockTicker);

            if (stock == null)
            {
                await InlineReply(Context.Message, $"There are no stocks with Ticker: `{stockTicker}`");
                return;
            }
            
            var account =  await _bankingService.GetOrCreateAccount(Context.User);

            var price = stock.Price * amount;
            
            if (!account.CanAfford(price))
            {
                await InlineReply(Context.Message, $"You don't have enough funds to buy that amount of `{stockTicker}` stock");
                return;
            }

            await _investingService.CreateInvestment(account.UserId, stock.Id, amount, stock.Price);
            await _bankingService.Withdraw(account, price);

            await ReplyAsync($"Investment made! {Context.User.Mention} bought `{amount}` of `{stock.Ticker}` priced at `{_options.Currency}{stock.Price:N2}`, making a total investment of `{_options.Currency}{price:N2}`");
        }
        
        // TODO: This needs testing
        [Command("sell")]
        [Summary("Sell a specific amount of stock from your portfolio.")]
        public async Task Sell([Summary("The portfolio ID of the stock you wish to sell.")] int portfolioId, 
            [Summary("The amount you wish to sell.")] int amount)
        {
            if (amount <= 0)
            {
                await InlineReply(Context.Message, "You have to sell at least 1 of a stock");
                return;
            }
            
            var investment = await _investingService.GetInvestmentById(portfolioId);

            if (investment == null || investment.UserId != Context.User.Id)
            {
                await InlineReply(Context.Message, $"You don't have any portfolio items with ID: `{portfolioId}`");
                return;
            }

            if (investment.Amount < amount)
            {
                await InlineReply(Context.Message, "You can't sell more than the amount you have in that stock");
                return;
            }
            
            var stock = await _investingService.GetStockById(investment.StockId);
            
            var account =  await _bankingService.GetOrCreateAccount(Context.User);

            var returns = stock.Price * amount;

            await _bankingService.Deposit(account, returns);

            if (amount == investment.Amount)
            {
                await _investingService.DeleteInvestment(investment);
            }
            else
            {
                investment.Amount -= amount;
                await _investingService.UpdateInvestment(investment);
            }
            
            await ReplyAsync($"Stock sold! {Context.User.Mention} sold `{amount}` `{stock.Ticker}` stock priced at `{_options.Currency}{stock.Price:N2}`, making a total sale of `{_options.Currency}{returns:N2}`");
        }

        [Command("portfolio")]
        [Summary("View your investment portfolio.")]
        public async Task Portfolio()
        {
            var portfolio = await _investingService.GetPortfolioByUserId(Context.User.Id);

            if (portfolio == null)
            {
                await InlineReply(Context.Message, $"You have no investments, use `{_options.Prefix}buy` to buy stocks");
                return;
            }
            
            var embed = BaseBankingEmbed()
                .WithTitle("Investment Portfolio")
                .WithDescription($"Use `{_options.Prefix}buy` to buy stocks and `{_options.Prefix}sell` to sell.");
            
            var portfolioText = portfolio.Aggregate("", (current, portfolioItem) 
                => current + $"• {portfolioItem.ToFormattedString(_options.Currency)}\n");
            
            embed.AddField("Portfolio", portfolioText);

            await ReplyAsync("", false, embed.Build());
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