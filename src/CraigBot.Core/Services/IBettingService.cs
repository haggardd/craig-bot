using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using Discord;

namespace CraigBot.Core.Services
{
    public interface IBettingService
    {
        Task<IEnumerable<Bet>> GetAllActiveBets();

        Task<Bet> GetActiveBetById(int id);

        Task<IEnumerable<Wager>> GetWagersByBetId(int id);

        Task<Bet> CreateBet(IUser user, string description, string forOdds, string againstOdds);

        Task<Wager> CreateWager(IUser user, int betId, decimal stake, bool inFavour);

        Task<BetResult> EndBet(Bet bet, bool result);

        Task VoidBet(Bet bet);
    }
}