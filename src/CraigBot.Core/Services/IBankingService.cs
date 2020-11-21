using System.Threading.Tasks;
using CraigBot.Core.Models;
using Discord.WebSocket;

namespace CraigBot.Core.Services
{
    public interface IBankingService
    {
        Task<BankAccount> GetAccount(SocketUser user);
        
        Task<BankAccount> CreateAccount(SocketUser user);

        Task<BankAccount> DepositToAccount(SocketUser user, decimal amount);

        Task OnMessageReceived(SocketMessage socketMessage);
    }
}