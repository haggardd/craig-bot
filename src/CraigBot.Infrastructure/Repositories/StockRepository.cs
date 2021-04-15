using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        public async Task<IEnumerable<Stock>> GetAll()
        {
            await using var context = new CraigBotDbContext();
            
            var stocks = await AsyncEnumerable.ToListAsync(context.Stocks);

            return stocks;
        }

        public async Task<IEnumerable<Stock>> GetAllByIds(IEnumerable<int> ids)
        {
            await using var context = new CraigBotDbContext();

            var stockIds = ids.ToList();

            var stocks = context.Stocks
                .AsQueryable()
                .Where(x => stockIds.Contains(x.Id))
                .ToList();

            return stocks;
        }

        public async Task<Stock> GetById(int id)
        {
            await using var context = new CraigBotDbContext();
            
            var stock = await context.Stocks.FindAsync(id);

            return stock;
        }

        public async Task<Stock> GetByTicker(string ticker)
        {
            await using var context = new CraigBotDbContext();

            var stock = await context.Stocks
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Ticker == ticker);

            return stock;
        }

        public async Task UpdateAll(IEnumerable<Stock> stocks)
        {
            await using var context = new CraigBotDbContext();
            
            context.Stocks.UpdateRange(stocks);

            await context.SaveChangesAsync();
        }
    }
}