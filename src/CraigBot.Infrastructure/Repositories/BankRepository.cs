using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly CraigBotDbContext _context;
        
        public BankRepository(CraigBotDbContext context)
        {
            _context = context;
        }

        public Task<Bank> GetByUserId(ulong id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Create(Bank bank)
        {
            _context.Banks.Add(bank);

            await _context.SaveChangesAsync();
        }
    }
}