using CraigBot.Bot.Common;

namespace CraigBot.Bot.Helpers
{
    public static class BetHelpers
    {
        public static decimal CalculateWinnings(this decimal wager, Fraction odds)
            => wager * odds.Numerator / odds.Denominator;
    }
}