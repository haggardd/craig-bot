using CraigBot.Core.Models;

namespace CraigBot.Core.Mappers
{
    public static class WagerMappers
    {
        public static WagerResult ToWagerResult(this Wager wager, decimal returns)
        {
            return new WagerResult
            {
                Username = wager.Username,
                Returns = returns,
                InFavour = wager.InFavour
            };
        }
    }
}