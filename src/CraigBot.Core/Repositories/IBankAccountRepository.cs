using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetByUserId(ulong id);
        
        Task<BankAccount> Create(BankAccount account);
        
        Task<BankAccount> Update(BankAccount bankAccount);
    }
}