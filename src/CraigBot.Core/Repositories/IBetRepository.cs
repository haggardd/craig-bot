using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAllBets();

        Task<Bet> GetBetById(int id);

        Task<IEnumerable<Wager>> GetWagersByBetId(int id);

        Task<Bet> CreateBet(Bet bet);

        Task<Wager> CreateWager(Wager wager);

        Task<Bet> UpdateBet(Bet bet);
    }
}