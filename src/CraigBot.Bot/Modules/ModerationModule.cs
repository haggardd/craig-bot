using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Summary("Moderation Commands")]
    public class ModerationModule : CraigBotBaseModule
    {
        #region Commands
        
        [Command("kick")]
        [Summary("Kicks the given user from the server.")]
        [Example("kick @Craig")]
        [Example("kick @Craig spamming")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [PreventCraigMention]
        public async Task Kick([Summary("The user you wish to kick.")] SocketGuildUser user,
            [Remainder][Summary("The reason for the kick.")] string reason = null)
            => await user.KickAsync(reason);

        [Command("ban")]
        [Summary("Bans the given user from the server.")]
        [Example("ban @Craig")]
        [Example("ban @Craig repeat spamming")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [PreventCraigMention]
        public async Task Ban([Summary("The user you wish to ban.")] SocketGuildUser user,
            [Remainder][Summary("The reason for the ban.")] string reason = null) 
            => await user.BanAsync(7, reason);

        [Command("warn")]
        [Summary("Warns the given user from the server.")]
        [Example("warn @Craig No caps please!")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [PreventCraigMention]
        public async Task Warn([Summary("The user you wish to warn.")] SocketGuildUser user, 
            [Remainder][Summary("The reason for the warning.")] string reason)
            => await ReplyAsync($"{user.Mention}, this is a warning! Reason: *{reason}*");

        [Command("mute")]
        [Summary("Server mutes the given user.")]
        [Example("mute @Craig")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task Mute([Summary("The user you wish to server mute.")] SocketGuildUser user)
            => await user.ModifyAsync(x => { x.Mute = true; });
        
        [Command("unmute")]
        [Summary("Un-mutes the given user.")]
        [Example("unmute @Craig")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task Unmute([Summary("The user you wish to un-mute.")] SocketGuildUser user)
            => await user.ModifyAsync(x => { x.Mute = false; });
        
        [Command("deafen")]
        [Summary("Deafens the given user.")]
        [Example("deafen @Craig")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        public async Task Deafen([Summary("The user you wish to deafen.")] SocketGuildUser user)
            => await user.ModifyAsync(x => { x.Deaf = true; });
        
        [Command("undeafen")]
        [Summary("Deafens the given user.")]
        [Example("undeafen @Craig")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        public async Task Undeafen([Summary("The user you wish to un-deafen.")] SocketGuildUser user)
            => await user.ModifyAsync(x => { x.Deaf = false; });

        #endregion
    }
}