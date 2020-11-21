using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Services
{
    public class BankingService : IBankingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly BotOptions _options;
        
        public BankingService(DiscordSocketClient discord, IBankAccountRepository bankAccountRepository, 
            IOptions<BotOptions> options)
        {
            _discord = discord;
            _bankAccountRepository = bankAccountRepository;
            _options = options.Value;

            _discord.MessageReceived += OnMessageReceived;
        }

        public async Task<BankAccount> GetAccount(SocketUser user)
        {
            var account = await _bankAccountRepository.GetByUserId(user.Id) ?? await CreateAccount(user);

            return account;
        }

        public async Task<BankAccount> CreateAccount(SocketUser user)
        {
            var account = new BankAccount
            {
                UserId = user.Id,
                Username = user.Username,
                Balance = _options.StartingBalance
            };

            var newAccount = await _bankAccountRepository.Create(account);
            
            return newAccount;
        }

        public async Task<BankAccount> DepositToAccount(SocketUser user, decimal amount)
        {
            var account = await GetAccount(user);

            account.Balance += amount;

            var updatedAccount = await _bankAccountRepository.Update(account);

            return updatedAccount;
        }

        public async Task OnMessageReceived(SocketMessage socketMessage)
        {
            var message = socketMessage as SocketUserMessage;

            if (message == null || message.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var argPos = 0;
            
            if (message.HasStringPrefix(_options.Prefix, ref argPos))
            {
                return;
            }

            await DepositToAccount(message.Author, _options.MessageReward);
        }
    }
}