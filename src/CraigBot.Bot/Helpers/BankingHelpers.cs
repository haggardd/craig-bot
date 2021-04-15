using System;
using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class BankingHelpers
    {
        public static bool CanAfford(this BankAccount account, decimal amount)
            => account.Balance - amount > 0;

        public static bool IsBelowMinimum(decimal amount)
            => amount < MinimumAmount;
        
        public static bool IsOutOfDate(this Stock stock, int updateRate)
        {
            var timespan = DateTime.Now - stock.LastUpdate;

            return timespan.Minutes > updateRate;
        }

        public const decimal MinimumAmount = 0.01M;

        public static decimal CalculateNextPrice(this Stock stock)
        {
            // https://en.wikipedia.org/wiki/Geometric_Brownian_motion
            // https://github.com/crodriguezvega/geometric-brownian-motion/blob/master/src/ViewModels/ViewModel.cs

            // TODO: Test 'Math.Ceiling' works as intended
            var range = stock.Price / 4;
            var highEnd = (int) (stock.Price + Math.Ceiling(range));
            var lowEnd = (int) (stock.Price - Math.Ceiling(range));
            
            var rng = new Random();
            
            return rng.Next(lowEnd, highEnd);
        }

        public static string ToFormattedString(this Stock stock, char currency)
        {
            var arrow = stock.PreviousPrice < stock.Price 
                ? "↑" 
                : "↓";

            const int padWidth = 10;
            const char padChar = ' ';
            
            var priceText = $"{currency}{stock.Price:N2}".PadRight(padWidth, padChar);
            var highText = $"{currency}{stock.High:N2}".PadRight(padWidth, padChar);
            var lowText = $"{currency}{stock.Low:N2}".PadRight(padWidth, padChar);
            
            return $"`{stock.Ticker}` |  Price: `{arrow} {priceText}` | High: `{highText}` | Low: `{lowText}`";
        }
        
        public static string ToFormattedString(this PortfolioItem item, char currency)
        {
            var buyTotal = item.Amount * item.BuyPrice;
            var currentTotal = item.Amount * item.CurrentPrice;
            var change = currentTotal - buyTotal;
            
            const char padChar = ' ';
            const int smallPad = 3;
            const int bigPad = 10;
            
            var formatting = $"+{currency}0.00;-{currency}0.00;{currency}0.00";
            
            var idText = $"{item.Id}".PadRight(smallPad, padChar);
            var amountText = $"{item.Amount}".PadRight(smallPad, padChar);
            var buyTotalText = $"{currency}{buyTotal:N2}".PadRight(bigPad, padChar);
            var currentTotalText = $"{currency}{currentTotal:N2}".PadRight(bigPad, padChar);
            var changeText = change.ToString(formatting).PadRight(bigPad, padChar);
            
            return $"`{idText}` | Stock: `{item.StockTicker}` |  Amount: `{amountText}` | Current: `{currentTotalText}` | Buy: `{buyTotalText}` | Change: `{changeText}`";
        }
    }
}