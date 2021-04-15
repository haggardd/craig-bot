using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IWagerRepository
    {
        Task<IEnumerable<Wager>> GetAllByBetId(int id);

        Task<Wager> Create(Wager wager);
        
        Task DeleteRange(IEnumerable<Wager> wagers);
    }
}