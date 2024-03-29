﻿using System;
using CraigBot.Bot.Common;
using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class BettingHelpers
    {
        public static decimal CalculateWinnings(this decimal wager, Fraction odds)
            => Math.Round(wager * odds.Numerator / odds.Denominator, 2);

        public static string ToFormattedString(this Bet bet)
            => $"Bet ID: `{bet.Id}` |  Odds: `{bet.ForOdds} <> {bet.AgainstOdds}` | Creator: `{bet.Username}`";

        public static string ToFormattedString(this Wager wager, char currency)
        {
            var inFavourText = wager.InFavour
                ? "In Favour"
                : "Against";
            
            return $"User: `{wager.Username}` | Stake: `{currency}{wager.Stake:N2}` | `{inFavourText}`";
        }
        
        public static string ToFormattedString(this WagerResult result, char currency)
        {
            var inFavourText = result.InFavour
                ? "In Favour"
                : "Against";
            
            return $"User: `{result.Username}` | Returns: `{currency}{result.Returns:N2}` | `{inFavourText}`";
        }
    }
}