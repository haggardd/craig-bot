using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly CraigBotDbContext _context;

        public BetRepository(CraigBotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bet>> GetAllBets()
        {
            var bets = await _context.Bets.ToListAsync();

            return bets;
        }

        public async Task<Bet> GetBetById(int id)
        {
            var bet = await _context.Bets.FindAsync(id);

            return bet;
        }

        // TODO: Might be a better way to do this
        public async Task<IEnumerable<Wager>> GetWagersByBetId(int id)
        {
            var wagers = await _context.Wagers.ToListAsync();

            var relevantWagers = wagers.Where(x => x.BetId == id);

            return relevantWagers;
        }

        public async Task<Bet> CreateBet(Bet bet)
        {
            var newBet = (await _context.Bets.AddAsync(bet)).Entity;

            await _context.SaveChangesAsync();

            return newBet;
        }

        public async Task<Wager> CreateWager(Wager wager)
        {
            var newWager = (await _context.Wagers.AddAsync(wager)).Entity;

            await _context.SaveChangesAsync();

            return newWager;
        }

        public async Task<Bet> UpdateBet(Bet bet)
        {
            var updatedBet = _context.Update(bet).Entity;

            await _context.SaveChangesAsync();

            return updatedBet;
        }
    }
}