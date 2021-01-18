using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using Discord;
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

        public async Task<BankAccount> GetAccount(ulong id)
        {
            var account = await _bankAccountRepository.GetByUserId(id);

            return account;
        }

        public async Task<BankAccount> GetOrCreateAccount(IUser user)
        {
            var account = await _bankAccountRepository.GetByUserId(user.Id) ?? await CreateAccount(user);

            return account;
        }

        public async Task<BankAccount> CreateAccount(IUser user)
        {
            var account = new BankAccount
            {
                UserId = user.Id,
                Username = user.Username,
                Balance = _options.StartingBalance < 0.01M 
                    ? 0.00M
                    : _options.StartingBalance
            };

            var newAccount = await _bankAccountRepository.Create(account);
            
            return newAccount;
        }

        public async Task<BankAccount> Deposit(BankAccount account, decimal amount)
        {
            account.Balance += amount;

            var updatedAccount = await _bankAccountRepository.Update(account);

            return updatedAccount;
        }

        public async Task<BankAccount> Withdraw(BankAccount account, decimal amount)
        {
            account.Balance -= amount;
            
            var updatedAccount = await _bankAccountRepository.Update(account);

            return updatedAccount;
        }

        public async Task OnMessageReceived(IMessage message)
        {
            if (_options.MessageReward < 0.01M)
            {
                return;
            }

            if (!(message is SocketUserMessage userMessage) 
                || userMessage.Author.Id == _discord.CurrentUser.Id)
            {
                return;
            }
            
            var argPos = 0;
            
            if (userMessage.HasStringPrefix(_options.Prefix, ref argPos))
            {
                return;
            }

            var account = await GetOrCreateAccount(userMessage.Author);

            await Deposit(account, _options.MessageReward);
        }
    }
}