using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class BankingHelpers
    {
        public static bool CanAfford(this BankAccount account, decimal amount)
            => account.Balance - amount > 0;

        public static bool IsBelowMinimum(decimal amount)
            => amount < MinimumAmount;

        public const decimal MinimumAmount = 0.01M;
    }
}