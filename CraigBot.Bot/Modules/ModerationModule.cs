using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Summary("Moderation Commands")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        // TODO: Finish implementing commands for the moderation module

        #region Commands

        [Command("kick")]
        [Summary("Kicks the given user from the server.")]
        public async Task Kick(SocketGuildUser user)
        {
        }
        
        [Command("ban")]
        [Summary("Bans the given user from the server.")]
        public async Task Ban(SocketGuildUser user)
        {
        }
        
        #endregion
    }
}