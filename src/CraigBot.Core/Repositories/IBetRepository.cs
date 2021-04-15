using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IBetRepository
    {
        Task<IEnumerable<Bet>> GetAll();

        Task<Bet> GetBetById(int id);

        Task<Bet> Create(Bet bet);

        Task<Bet> Update(Bet bet);
    }
}