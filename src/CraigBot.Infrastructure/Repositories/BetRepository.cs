using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    // TODO: You should delete or filter old bets at the repo level 
    public class BetRepository : IBetRepository
    {
        public async Task<IEnumerable<Bet>> GetAll()
        {
            await using var context = new CraigBotDbContext();
            
            var bets = await context.Bets.ToListAsync();

            return bets;
        }

        public async Task<Bet> GetBetById(int id)
        {
            await using var context = new CraigBotDbContext();
            
            var bet = await context.Bets.FindAsync(id);

            return bet;
        }

        public async Task<Bet> Create(Bet bet)
        {
            await using var context = new CraigBotDbContext();
            
            var newBet = (await context.Bets.AddAsync(bet)).Entity;

            await context.SaveChangesAsync();

            return newBet;
        }
        
        public async Task<Bet> Update(Bet bet)
        {
            await using var context = new CraigBotDbContext();
            
            var updatedBet = context.Bets.Update(bet).Entity;

            await context.SaveChangesAsync();

            return updatedBet;
        }
    }
}