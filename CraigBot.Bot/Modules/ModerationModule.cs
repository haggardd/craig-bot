using System.Threading.Tasks;
using CraigBot.Bot.Common;
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
        [Example("!kick @Craig")]
        public async Task Kick([Summary("The user you wish to kick.")] SocketGuildUser user)
            => await ReplyAsync("Not implemented yet! Hold your horses!");

        [Command("ban")]
        [Summary("Bans the given user from the server.")]
        [Example("!ban @Craig")]
        public async Task Ban([Summary("The user you wish to ban.")] SocketGuildUser user)
            => await ReplyAsync("Not implemented yet! Hold your horses!");
        
        [Command("unban")]
        [Summary("Un-bans the given user.")]
        [Example("!unban @Craig")]
        public async Task Unban([Summary("The user you wish to un-ban.")] SocketGuildUser user)
            => await ReplyAsync("Not implemented yet! Hold your horses!");
        
        [Command("warn")]
        [Summary("Warns the given user from the server.")]
        [Example("!warn @Craig")]
        [Example("!warn @Craig No caps please!")]
        public async Task Warn([Summary("The user you wish to warn.")] SocketGuildUser user, 
            [Remainder] [Summary("The reason for the warning.")] string reason)
            => await ReplyAsync("Not implemented yet! Hold your horses!");
        
        [Command("mute")]
        [Summary("Server mutes the given user.")]
        [Example("!mute @Craig")]
        [Example("!mute @Craig Spamming mic")]
        public async Task Mute([Summary("The user you wish to server mute.")] SocketGuildUser user, 
            [Remainder] [Summary("The reason for muting.")] string reason = null)
            => await ReplyAsync("Not implemented yet! Hold your horses!");
        
        [Command("unmute")]
        [Summary("Un-mutes the given user.")]
        [Example("!unmute @Craig")]
        public async Task Unmute([Summary("The user you wish to un-mute.")] SocketGuildUser user)
            => await ReplyAsync("Not implemented yet! Hold your horses!");

        #endregion
    }
}