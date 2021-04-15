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
    [Summary("Betting Commands")]
    public class BettingModule : CraigBotBaseModule
    {
        private readonly IBankingService _bankingService;
        private readonly IBettingService _bettingService;
        private readonly BotOptions _options;

        public BettingModule(IBankingService bankingService, IBettingService bettingService, IOptions<BotOptions> options)
        {
            _bankingService = bankingService;
            _bettingService = bettingService;
            _options = options.Value;
        }

        #region Commands
        
        [Command("bets")]
        [Summary("List all active bets")]
        public async Task Bets()
        {
            var bets = (await _bettingService.GetAllActiveBets()).ToList();

            if (!bets.Any())
            {
                await InlineReply(Context.Message, "There are no active bets");
                return;
            }

            var embed = BaseBettingEmbed()
                .WithTitle("Active Bets");

            foreach (var bet in bets)
            {
                var betTitle = bet.ToFormattedString();
                embed.AddField(betTitle, bet.Description);
            }
            
            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("bet")]
        [Summary("View an active bet and its wagers.")]
        [Example("bet 4")]
        public async Task Bet([Summary("The ID of the bet you wish to view.")] int betId)
        {
            var bet = await _bettingService.GetActiveBetById(betId);

            if (bet == null)
            {
                await InlineReply(Context.Message, $"There are no active bets with ID: `{betId}`");
                return;
            }
            
            var wagers = (await _bettingService.GetWagersByBetId(betId)).ToList();

            var embed = BaseBettingEmbed()
                .WithTitle($"Bet ID: `{bet.Id}`")
                .WithDescription($"Use `{_options.Prefix}wager` to participate.")
                .AddField("Description", bet.Description)
                .AddField("Odds", $"`{bet.ForOdds} <> {bet.AgainstOdds}`");

            var wagersInfo = "No wagers for this bet.";

            if (wagers.Any())
            {
                wagersInfo = wagers.Aggregate("", (current, wager) 
                    => current + $"• {wager.ToFormattedString(_options.Currency)}\n");
            }

            embed.AddField("Wagers", wagersInfo);

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
            var bet = await _bettingService.CreateBet(Context.User, description, forOdds.ToString(), againstOdds.ToString());

            var embed = BaseBettingEmbed()
                .WithTitle($"Bet ID: `{bet.Id}`")
                .WithDescription($"Use `{_options.Prefix}wager` to participate.")
                .AddField("Description", bet.Description)
                .AddField("Odds", $"`{bet.ForOdds} <> {bet.AgainstOdds}`");

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("wager")]
        [Summary("Create a new wager for an active bet.")]
        [Example("wager 4 100.00 true")]
        [Example("wager 2 10.10 false")]
        [Example("wager 10 0.1 false")]
        public async Task Wager([Summary("The ID of the bet you're betting on.")] int betId, 
            [Summary("Your stake in the bet")] decimal stake, 
            [Summary("If you're betting in favour or against the bet.")] bool inFavour)
        {
            var bet = await _bettingService.GetActiveBetById(betId);

            if (bet == null)
            {
                await InlineReply(Context.Message, $"There are no active bets with ID: `{betId}`");
                return;
            }
            
            var account =  await _bankingService.GetOrCreateAccount(Context.User);
            
            if (BankingHelpers.IsBelowMinimum(stake))
            {
                await InlineReply(Context.Message, $"The minimum amount you can stake is `{_options.Currency}{BankingHelpers.MinimumAmount:N2}`");
                return;
            }
            
            if (!account.CanAfford(stake))
            {
                await InlineReply(Context.Message, "You don't have enough funds to make that bet");
                return;
            }
        
            await _bankingService.Withdraw(account, stake);
            await _bettingService.CreateWager(Context.User, betId, stake, inFavour);
            
            await ReplyAsync($"Wager placed! {Context.User.Mention} wagered `{_options.Currency}{stake:N2}` on bet ID: `{bet.Id}`");
        }

        [Command("result")]
        [Summary("Ends a given bet with the provided result, gives a rundown of the bet and cashes out wagers.")]
        [Example("result 5 true")]
        [Example("result 1 false")]
        public async Task Result([Summary("The bet you wish to end.")] int betId,
            [Summary("The result of the bet")] bool result)
        {
            var bet = await _bettingService.GetActiveBetById(betId);

            if (bet == null)
            {
                await InlineReply(Context.Message, $"There are no active bets with ID: `{betId}`");
                return;
            }
            
            if (bet.UserId != Context.User.Id)
            {
                await InlineReply(Context.Message, $"You can't end bets you didn't create");
                return;
            }

            var betResult = await _bettingService.EndBet(bet, result);
            
            var resultMessage = result
                ? "The bet came through!"
                : "The bet went against the odds!";

            var embed = BaseBettingEmbed()
                .WithTitle($"Bet Result ID: `{bet.Id}`")
                .WithDescription($"{bet.Description}")
                .AddField("Odds", $"`{bet.ForOdds} <> {bet.AgainstOdds}`", true)
                .AddField("Result", resultMessage, true);

            var wagerResults = "No wagers were made.";
            
            if (betResult.WagerResults.Any())
            {
                wagerResults = betResult.WagerResults.Aggregate("", (current, wagerResult) 
                    => current + $"• {wagerResult.ToFormattedString(_options.Currency)}\n");
            }

            embed.AddField("Wager Results", wagerResults);
            
            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("void")]
        [Summary("Voids a given bet and refunds wagers.")]
        [Example("void 3")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Void([Summary("The bet you wish to void.")] int betId)
        {
            var bet = await _bettingService.GetActiveBetById(betId);

            if (bet == null)
            {
                await InlineReply(Context.Message, $"There are no active bets with ID: `{betId}`");
                return;
            }

            await _bettingService.VoidBet(bet);
            
            await ReplyAsync($"Bet ID: `{betId}` has been voided! All wagers have been refunded");
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