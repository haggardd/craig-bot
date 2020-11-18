using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using Discord.WebSocket;

namespace CraigBot.Bot.Services
{
    public class BankingService : IBankingService
    {
        private readonly DiscordSocketClient _socketClient;
        private readonly IBankRepository _bankRepository;
        
        public BankingService(DiscordSocketClient socketClient, IBankRepository bankRepository)
        {
            _socketClient = socketClient;
            _bankRepository = bankRepository;
        }

        public async Task Create(SocketUser user)
        {
            var newBank = new Bank
            {
                UserId = user.Id,
                Username = user.Username,
                Balance = 1000 // TODO: Need to decided how the starting balance is set and how much
            };

            await _bankRepository.Create(newBank);
        }
    }
}