using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Repositories
{
    public class InvestmentRepository : IInvestmentRepository
    {
        public async Task<Investment> GetByInvestmentId(int id)
        {
            await using var context = new CraigBotDbContext();
            
            var investment = await context.Investments.FindAsync(id);

            return investment;
        }

        public async Task<IEnumerable<Investment>> GetAllByUserId(ulong id)
        {
            await using var context = new CraigBotDbContext();

            var investments = await context.Investments
                .AsQueryable()
                .Where(x => x.UserId == id)
                .ToListAsync();

            return investments;
        }

        public async Task<Investment> Create(Investment investment)
        {
            await using var context = new CraigBotDbContext();
            
            var newInvestment = (await context.Investments.AddAsync(investment)).Entity;

            await context.SaveChangesAsync();

            return newInvestment;
        }

        public async Task<Investment> Update(Investment investment)
        {
            await using var context = new CraigBotDbContext();
            
            var updatedInvestment = context.Investments.Update(investment).Entity;

            await context.SaveChangesAsync();

            return updatedInvestment;
        }

        public async Task Delete(Investment investment)
        {
            await using var context = new CraigBotDbContext();

            context.Investments.Remove(investment);
            
            await context.SaveChangesAsync();
        }
    }
}