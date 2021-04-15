using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        public async Task<BankAccount> GetByUserId(ulong id)
        {
            await using var context = new CraigBotDbContext();
            
            var bank = await context.BankAccounts.SingleOrDefaultAsync(x => x.UserId == id);

            return bank;
        }

        public async Task<BankAccount> Create(BankAccount account)
        {
            await using var context = new CraigBotDbContext();
            
            var newBank = (await context.BankAccounts.AddAsync(account)).Entity;

            await context.SaveChangesAsync();

            return newBank;
        }

        public async Task<BankAccount> Update(BankAccount account)
        {
            await using var context = new CraigBotDbContext();
            
            var updatedAccount = context.BankAccounts.Update(account).Entity;

            await context.SaveChangesAsync();

            return updatedAccount;
        }
    }
}