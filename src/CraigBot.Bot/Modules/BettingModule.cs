using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Common;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    [Summary("Betting Commands")]
    public class BettingModule : CraigBotBaseModule
    {
        private readonly IBankingService _bankingService;
        private readonly IBetService _betService;
        private readonly BotOptions _options;

        public BettingModule(IBankingService bankingService, IBetService betService, IOptions<BotOptions> options)
        {
            _bankingService = bankingService;
            _betService = betService;
            _options = options.Value;
        }

        #region Commands

        [Command("bets")]
        [Summary("List all active bets")]
        public async Task Bets()
        {
            var bets = (await _betService.GetAllActiveBets()).ToList();

            if (!bets.Any())
            {
                // TODO: Might want to look at a way to make these generic replies prettier
                await ReplyAsync("There are no active bets.");
                return;
            }

            var embed = BaseBettingEmbed()
                .WithTitle("Active Bets");

            foreach (var bet in bets)
            {
                // TODO: Might be a good idea to create some custom `ToString()` methods for stuff like this (and not just bets)
                // TODO: Need to make sure text like this is consistent across the codebase
                var betTitle = $"Bet `ID: {bet.Id}` |  Odds `{bet.ForOdds} | {bet.AgainstOdds}` | Creator `{bet.Username}`";
                embed.AddField(betTitle, bet.Description);
            }
            
            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("bet")]
        [Summary("Create a new custom bet.")]
        [Example("bet 1/2 1/2 Will we win this game?")]
        [Example("bet 1000/1 10/1 Will Craig be kicked from this guild tonight?")]
        public async Task Bet([Summary("The fractional odds for this bet.")] Fraction forOdds,
            [Summary("The fractional odds against this bet.")] Fraction againstOdds,
            [Remainder][Summary("The description for this bet.")] string description)
        {
            var bet = await _betService.CreateBet(Context.User, description, forOdds.ToString(), againstOdds.ToString());

            var embed = BaseBettingEmbed()
                .WithTitle($"Bet `ID: {bet.Id}`")
                .WithDescription($"Use `{_options.Prefix}wager` to participate.")
                .AddField("Description", bet.Description)
                .AddField("Odds", $"`{bet.ForOdds} | {bet.AgainstOdds}`");

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Might need to change how `inFavour` is passed through
        [Command("wager")]
        [Summary("Create a new wager for an active bet.")]
        [Example("wager 4 100.00 true")]
        [Example("wager 2 10.10 false")]
        [Example("wager 10 0.1 false")]
        public async Task Wager([Summary("The ID of the bet you're betting on.")] int betId, 
            [Summary("Your stake in the bet")] decimal stake, 
            [Summary("If you're betting in favour or against the bet.")] bool inFavour)
        {
            var bet = await _betService.GetActiveBetById(betId);

            if (bet == null)
            {
                await ReplyAsync($"There are active bets with ID: `{betId}`.");
                return;
            }
            
            var account =  await _bankingService.GetOrCreateAccount(Context.User);
            
            if (BankingHelpers.BelowMinimum(stake))
            {
                await ReplyAsync($"The minimum amount you can stake is `{_options.Currency}{BankingHelpers.MinimumAmount}`!");
                return;
            }
            
            if (!account.CanAfford(stake))
            {
                // TODO: Need to make sure responses are consistent across the codebase
                await ReplyAsync("You don't have enough funds to make that bet!");
                return;
            }
        
            await _bankingService.Withdraw(account, stake);
            // TODO: Need to decide if multiple wagers for the same bet should be allowed
            await _betService.CreateWager(Context.User, betId, stake, inFavour);
            
            await ReplyAsync($"Wager placed! {Context.User.Mention} wagered `{_options.Currency}{stake:0.00}` on bet ID: `{bet.Id}`.");
        }

        [Command("result")]
        [Summary("Ends a given bet with the provided result, gives a rundown of the bet and cashes out wagers.")]
        [Example("result 5 true")]
        [Example("result 1 false")]
        public async Task Result([Summary("The bet you wish to end.")] int betId,
            [Summary("The result of the bet")] bool result)
        {
            var bet = await _betService.GetActiveBetById(betId);

            if (bet == null)
            {
                await ReplyAsync($"There are active bets with ID: `{betId}`.");
                return;
            }

            if (bet.UserId != Context.User.Id)
            {
                await ReplyAsync($"You aren't the creator of this bet.");
                return;
            }

            var betResult = await _betService.EndBet(bet, result);

            // TODO: Not sure about the wordage here
            var resultMessage = result
                ? "The bet came through!"
                : "The bet went against the odds!";

            var embed = BaseBettingEmbed()
                .WithTitle($"Bet Result `ID: {bet.Id}`")
                .WithDescription($"Creator `{bet.Username}`\n{bet.Description}")
                .AddField("Odds", $"{bet.ForOdds} | {bet.AgainstOdds}", true)
                .AddField("Result", resultMessage, true);

            var wagerResults = "";
            
            // TODO: Should check if anyone actually made wagers first
            foreach(var wagerResult in betResult.WagerResults)
            {
                var inFavourTest = wagerResult.InFavour
                    ? "`In Favour`"
                    : "`Against`";

                wagerResults += $"• `{wagerResult.Username}` | Winnings: `{wagerResult.Winnings}` | `{inFavourTest}`\n";
            }

            embed.AddField("Wager Results", wagerResults);
            
            await ReplyAsync("", false, embed.Build());
        }

        #endregion

        #region Helpers
        
        private EmbedBuilder BaseBettingEmbed()
            => new EmbedBuilder()
                .WithColor(Color.Magenta)
                .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl());

        #endregion
    }
}