using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Summary("Utility Commands")]
    public class UtilityModule : CraigBotBaseModule
    {
        #region Commands

        // TODO: May want to add more fields to this...
        [Command("inspect")]
        [Summary("Displays information about the server.")]
        public async Task Inspect()
        {
            var created = Context.Guild.CreatedAt.DateTime.ToString(CultureInfo.CurrentCulture);

            var embed = BaseUtilityEmbed()
                .WithTitle(Context.Guild.Name)
                .WithDescription(Context.Guild.Description)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .AddField("Created", created, true)
                .AddField("Creator", Context.Guild.Owner.Username, true)
                .AddField("Member Count", $"`{Context.Guild.MemberCount}`");

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Could benefit from adding more fields, i.e. current game, music, etc...
        [Command("inspect")] 
        [Summary("Displays information about a given user.")]
        [Example("inspect @Craig")]
        public async Task Inspect([Summary("The user you wish to inspect.")] SocketGuildUser user)
        {
            var joined = user.JoinedAt == null
                ? "Unknown..."
                : user.JoinedAt.Value.DateTime.ToString(CultureInfo.CurrentCulture);

            var roles = string.Join(" | ", user.Roles.Select(r => $"`{r.Name}`"));

            var embed = BaseUtilityEmbed()
                .WithTitle($"{user.Nickname ?? user.Username} - `{user.Status}`")
                .WithThumbnailUrl(user.GetAvatarUrl())
                .AddField("Joined", joined, true)
                .AddField("Is bot?", $"`{user.IsBot.ToString().ToUpper()}`", true)
                .AddField("Roles", roles);
            
            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("inspect")]
        [Summary("Displays information about a given text channel.")]
        [Example("inspect #general")]
        public async Task Inspect([Summary("The text channel you wish to inspect.")] SocketTextChannel channel)
        {
            var slowModeInterval = channel.SlowModeInterval == 0
                ? "None"
                : $"`{channel.SlowModeInterval}s`";

            var embed = BaseUtilityEmbed()
                .WithTitle($"#{channel.Name}")
                .WithDescription(channel.Topic ?? "No topic...")
                .AddField("Created", channel.CreatedAt.DateTime.ToString(CultureInfo.CurrentCulture))
                .AddField("Category", $"{channel.Category}", true)
                .AddField("NSFW?", $"`{channel.IsNsfw.ToString().ToUpper()}`", true)
                .AddField("Slow-Mode Interval", slowModeInterval);

            await ReplyAsync("", false, embed.Build());
        }
        
        [Command("ping")]
        [Summary("A test command, replies with 'Pong!'.")]
        public async Task Ping()
            => await MentionReply("Pong!");
        
        [Command("latency")]
        [Summary("Replies with the Bot's latency.")]
        public async Task Latency()
            => await MentionReply($"Latency: `{Context.Client.Latency}ms`");
        
        [Command("git")]
        [Summary("Replies with the Bot's GitHub repo.")]
        public async Task Git()
            => await MentionReply("Check out my codebase on Github! https://github.com/haggardd/craig-bot");

        #endregion

        #region Helpers

        private EmbedBuilder BaseUtilityEmbed()
            => new EmbedBuilder()
                .WithColor(Color.Blue);

        #endregion
    }
}