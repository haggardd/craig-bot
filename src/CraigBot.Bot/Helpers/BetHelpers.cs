using System;
using CraigBot.Bot.Common;
using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class BetHelpers
    {
        public static decimal CalculateWinnings(this decimal wager, Fraction odds)
            => Math.Round(wager * odds.Numerator / odds.Denominator, 2);

        public static string ToFormattedString(this Bet bet)
            // TODO: Need to make sure text like this is consistent across the codebase
            => $"Bet ID: `{bet.Id}` |  Odds: `{bet.ForOdds} <> {bet.AgainstOdds}` | Creator: `{bet.Username}`";

        public static string ToFormattedString(this WagerResult result, char currency)
        {
            var inFavourText = result.InFavour
                ? "In Favour"
                : "Against";
            
            return $"User: `{result.Username}` | Returns: `{currency}{result.Returns:0.00}` | `{inFavourText}`";
        }
    }
}