using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly CraigBotDbContext _context;
        
        public BankAccountRepository(CraigBotDbContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetByUserId(ulong id)
        {
            var bank = await _context.BankAccounts.SingleOrDefaultAsync(x => x.UserId == id);

            return bank;
        }

        public async Task<BankAccount> Create(BankAccount account)
        {
            var newBank = (await _context.BankAccounts.AddAsync(account)).Entity;

            await _context.SaveChangesAsync();

            return newBank;
        }

        public async Task<BankAccount> Update(BankAccount account)
        {
            var updatedAccount = _context.Update(account).Entity;

            await _context.SaveChangesAsync();

            return updatedAccount;
        }
    }
}