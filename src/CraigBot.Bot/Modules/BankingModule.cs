using System.Threading.Tasks;
using CraigBot.Core.Services;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    [Summary("Banking Commands")]
    [RequireContext(ContextType.Guild)]
    public class BankingModule : ModuleBase<SocketCommandContext>
    {
        private readonly IBankingService _bankingService;
        
        public BankingModule(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }
        
        #region Commands
        
        [Command("bank")]
        [Summary("Replies with the user's bank status.")]
        public async Task Bank()
        {
            await ReplyAsync("Not implemented yet...");
        }
        
        #endregion
    }
}