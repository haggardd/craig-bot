using System.Collections.Generic;
using CraigBot.Core.Models;

namespace CraigBot.Core.Mappers
{
    public static class BetMappers
    {
        public static BetResult ToBetResult(this Bet bet, IEnumerable<WagerResult> wagerResults)
        {
            return new BetResult
            {
                Username = bet.Username,
                Description = bet.Description,
                WagerResults = wagerResults
            };
        }
    }
}