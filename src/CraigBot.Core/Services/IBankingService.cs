using System.Threading.Tasks;
using CraigBot.Core.Models;
using Discord;

namespace CraigBot.Core.Services
{
    public interface IBankingService
    {
        Task<BankAccount> GetAccount(ulong id);
        
        Task<BankAccount> GetOrCreateAccount(IUser user);
           
        Task<BankAccount> CreateAccount(IUser user);

        Task<BankAccount> Deposit(BankAccount account, decimal amount);
        
        Task<BankAccount> Withdraw(BankAccount account, decimal amount);

        Task OnMessageReceived(IMessage message);
    }
}