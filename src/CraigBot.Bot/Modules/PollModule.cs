using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Poll Commands")]
    [RequireContext(ContextType.Guild)]
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        #region Commands

        // TODO: Finish implementing this
        /* Things to consider:
        *  - Calculate votes (this is proving to be tricky!)
        *  - Check who has already voted */
        [Command("poll", RunMode = RunMode.Async)]
        [Summary("Creates a channel wide poll with a set duration and up to 10 options.")]
        [Example("poll \"What shall we play?\" 10 \"CS:GO\" \"Red Dead\" \"Sea of Thieves\"")]
        public async Task Poll([Summary("The question / choice you'd like to start a poll for.")] string question, 
            [Summary("The amount of seconds to pass before the winner is decided.")] int duration, 
            [Summary("The options to pick from during the poll.")] params string[] options)
        {
            if (options.Length <= 1 || options.Length > 10)
            {
                await ReplyAsync("To start a poll you need between 2 and 10 options to choose from.");
                return;
            }

            var pollEmbed = new EmbedBuilder()
                .WithColor(Color.Gold)
                .WithTitle(question)
                .WithAuthor(Context.User)
                .WithFooter(f => f.Text = $"Poll ends {duration} seconds from message sent")
                .WithCurrentTimestamp();
            
            string optionsText = null;
            
            for (var i = 0; i < options.Length; i++)
            {
                optionsText += $"`{i + 1})` {options[i]} \n";
            }
            
            pollEmbed.AddField("Options: ", optionsText);
            
            var pollMessage = await ReplyAsync("Vote Now!", false, pollEmbed.Build());
            var emotes = _emoteNumbers;
            
            for (var i = 0; i < options.Length; i++)
            {
                await pollMessage.AddReactionAsync(emotes[i]);
            }
            
            await Task.Delay(duration * 1000);
            
            var pollEndEmbed = new EmbedBuilder()
                .WithColor(Color.Gold)
                .WithTitle("Poll Over!")
                .WithDescription(question)
                .AddField("Most Votes Received: ", options[0])
                .WithAuthor(Context.User);
            
            await ReplyAsync("", false, pollEndEmbed.Build());
        }
        
        #endregion
        
        #region Helpers

        private readonly List<Emoji> _emoteNumbers = new List<Emoji>
        {
            new Emoji("\u0031\u20E3"),
            new Emoji("\u0032\u20E3"),
            new Emoji("\u0033\u20E3"),
            new Emoji("\u0034\u20E3"),
            new Emoji("\u0035\u20E3"),
            new Emoji("\u0036\u20E3"),
            new Emoji("\u0037\u20E3"),
            new Emoji("\u0038\u20E3"),
            new Emoji("\u0039\u20E3"),
            new Emoji("\uD83D\uDD1F")
        };

        #endregion
    }
}