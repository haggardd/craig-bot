using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    // TODO: Improve error reporting for `MultipleMatches` error
    [Summary("Utility Commands")]
    [RequireContext(ContextType.Guild)]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        #region Commands

        // TODO: May want to add more fields to this...
        [Command("inspect")]
        [Summary("Displays information about the server.")]
        [Example("!inspect")]
        public async Task Inspect()
        {
            var created = Context.Guild.CreatedAt.DateTime.ToString(CultureInfo.CurrentCulture);

            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle(Context.Guild.Name)
                .WithDescription(Context.Guild.Description)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Created", created, true)
                .AddField("Creator", Context.Guild.Owner.Username, true)
                .AddField("Member Count", $"`{Context.Guild.MemberCount}`");

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Could benefit from added more fields, i.e. current game / music, etc...
        [Command("inspect")] 
        [Summary("Displays information about a given user.")]
        [Example("!inspect @Craig")]
        public async Task Inspect([Summary("The user you wish to inspect.")] SocketGuildUser user)
        {
            var joined = user.JoinedAt == null
                ? "Unknown..."
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
        
        // TODO: May want to add more fields to this...
        [Command("inspect")]
        [Summary("Displays information about a given text channel.")]
        [Example("!inspect #general")]
        public async Task Inspect([Summary("The text channel you wish to inspect.")] SocketTextChannel channel)
        {
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle($"#{channel.Name}")
                .WithDescription(channel.Topic ?? "No topic...")
                .AddField("Created", channel.CreatedAt.DateTime.ToString(CultureInfo.CurrentCulture))
                .AddField("Category", $"{channel.Category}", true)
                .AddField("NSFW?", $"`{channel.IsNsfw.ToString().ToUpper()}`", true);

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("ping")]
        [Summary("A test command, replies with 'pong'.")]
        [Example("!ping")]
        public async Task Ping()
            => await ReplyAsync("Pong!");
        
        [Command("latency")]
        [Summary("Replies with the Bot's latency.")]
        [Example("!ping")]
        public async Task Latency()
            => await ReplyAsync($"Latency: `{Context.Client.Latency}ms`");

        #endregion
    }
}