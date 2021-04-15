using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Repositories
{
    public class WagerRepository : IWagerRepository
    {
        public async Task<IEnumerable<Wager>> GetAllByBetId(int id)
        {
            await using var context = new CraigBotDbContext();

            var wagers = await context.Wagers
                .AsQueryable()
                .Where(x => x.BetId == id)
                .ToListAsync();

            return wagers;
        }

        public async Task<Wager> Create(Wager wager)
        {
            await using var context = new CraigBotDbContext();
            
            var newWager = (await context.Wagers.AddAsync(wager)).Entity;

            await context.SaveChangesAsync();

            return newWager;
        }

        public async Task DeleteRange(IEnumerable<Wager> wagers)
        {
            await using var context = new CraigBotDbContext();

            context.Wagers.RemoveRange(wagers);
            
            await context.SaveChangesAsync();
        }
    }
}