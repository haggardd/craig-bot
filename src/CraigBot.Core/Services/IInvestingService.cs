using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Services
{
    public interface IInvestingService
    {
        Task<IEnumerable<Stock>> GetAllStocks();

        Task<Investment> GetInvestmentById(int id);
        
        Task<IEnumerable<PortfolioItem>> GetPortfolioByUserId(ulong id);
        
        Task<Stock> GetStockById(int id);
        
        Task<Stock> GetStockByTicker(string ticker);

        Task<Investment> CreateInvestment(ulong userId, int stockId, int amount, decimal buyPrice);
        
        Task<Investment> UpdateInvestment(Investment investment);

        Task DeleteInvestment(Investment investment);
    }
}