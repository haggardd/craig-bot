using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Group("util")]
    [Summary("Utility Commands")]
    [RequireContext(ContextType.Guild)]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        #region Commands

        // TODO: Could benefit from added more fields, i.e. current game / music, etc...
        [Command("inspect")] 
        [Summary("Displays information about the give user.")]
        public async Task InspectUser(SocketGuildUser user)
        {
            var joined = user.JoinedAt == null
                ? "UNKNOWN"
                : user.JoinedAt.Value.DateTime.ToString(CultureInfo.CurrentCulture);

            var roles = string.Join(" | ", user.Roles.Select(r => $"`{r.Name}`"));

            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle($"{user.Nickname ?? user.Username} - `{user.Status}`")
                .WithThumbnailUrl(user.GetAvatarUrl())
                .AddField("Joined", joined, true)
                .AddField("Is bot?", $"`{user.IsBot.ToString().ToUpper()}`", true)
                .AddField("Roles", roles);
            
            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Add further inspect commands for channels, servers etc...

        [Command("ping")]
        [Summary("A test commands, replies with 'pong'.")]
        public async Task Ping()
            => await ReplyAsync("Pong!");

        #endregion
    }
}