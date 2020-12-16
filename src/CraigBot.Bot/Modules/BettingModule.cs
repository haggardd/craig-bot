using System;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Models;
using CraigBot.Core.Services;
using Discord.Commands;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    [Summary("Betting Commands")]
    [RequireContext(ContextType.Guild)]
    public class BettingModule : CraigBotBaseModule
    {
        private readonly IBankingService _bankingService;
        private readonly BotOptions _options;
        private readonly Random _random;
        
        public BettingModule(IBankingService bankingService, IOptions<BotOptions> options, Random random)
        {
            _bankingService = bankingService;
            _options = options.Value;
            _random = random;
        }

        #region Commands
        
        [Command("coin")]
        [Summary("Bet on a coin flip.")]
        [Example("bet flip heads 10")]
        [Example("bet flip tails 0.01")]
        [Example("bet flip heads 1000.1")]
        public async Task Flip([Summary("Your guess for the coin flip.")] string guess, 
            [Summary("The amount of funds you wish to bet.")] decimal bet)
        {
            var account =  await _bankingService.GetAccountOrCreateAccount(Context.User);
            
            if(!await CanMakeBet(account, bet))
            {
                return;
            }
            
            var lowerCaseGuess = guess.ToLower();
            
            if (!lowerCaseGuess.Equals("heads") && !lowerCaseGuess.Equals("tails"))
            {
                await ReplyAsync($"`{guess}` is not a valid guess, please use either `{Heads}` or `{Tails}`.");
                return;
            }
            
            var result = _random.Next(2) == 0 ? Heads : Tails;
            var wonBet = result.Equals(guess);

            if (wonBet)
            {
                await _bankingService.Deposit(account, bet);
                await ReplyAsync($"Nice one, it's {result}! You won `{_options.Currency}{bet:0.00}`!");
            }
            else
            {
                await _bankingService.Withdraw(account, bet);
                await ReplyAsync($"Unlucky, it was {result}! You lose `{_options.Currency}{bet:0.00}`.");
            }
        }

        [Command("slots")]
        [Summary("Try your luck on a slot machine.")]
        public async Task Slots([Summary("The amount of funds you wish to bet.")] decimal bet)
        {
            var account =  await _bankingService.GetAccountOrCreateAccount(Context.User);
            
            if(!await CanMakeBet(account, bet))
            {
                return;
            }
            
            await ReplyAsync("Not implemented!");
        }

        #endregion

        #region Helpers

        private const string Heads = "heads";

        private const string Tails = "tails";

        private async Task<bool> CanMakeBet(BankAccount account, decimal bet)
        {
            if (BankingHelpers.BelowMinimum(bet))
            {
                await ReplyAsync($"The minimum amount you can bet is `{_options.Currency}{BankingHelpers.MinimumAmount}`!");
                return false;
            }
            
            if (!BankingHelpers.CanAfford(account, bet))
            {
                // TODO: Need to make sure responses are consistent across the codebase
                await ReplyAsync("You don't have enough funds to make that bet!");
                return false;
            }

            return true;
        }

        #endregion
    }
}