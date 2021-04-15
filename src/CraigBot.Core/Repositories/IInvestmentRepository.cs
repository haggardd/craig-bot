using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IInvestmentRepository
    {
        Task<Investment> GetByInvestmentId(int id);
        
        Task<IEnumerable<Investment>> GetAllByUserId(ulong id);
        
        Task<Investment> Create(Investment investment);
        
        Task<Investment> Update(Investment investment);
        
        Task Delete(Investment investment);
    }
}