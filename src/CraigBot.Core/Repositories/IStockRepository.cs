using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAll();
        
        Task<IEnumerable<Stock>> GetAllByIds(IEnumerable<int> ids);
        
        Task<Stock> GetById(int id);
        
        Task<Stock> GetByTicker(string ticker);

        Task UpdateAll(IEnumerable<Stock> stocks);
    }
}