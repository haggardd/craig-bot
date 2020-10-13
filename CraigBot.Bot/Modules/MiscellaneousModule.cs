using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Group("misc")]
    [Summary("Miscellaneous Commands")]
    public class MiscellaneousModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _random;

        public MiscellaneousModule(Random random)
        {
            _random = random;
        }

        #region Commands

        [Command("roll")]
        [Summary("Rolls a 6 sided die.")]
        public async Task Roll()
            => await ReplyAsync($"Its {_random.Next(6) + 1}!");
        
        // TODO: Look into preventing numbers over int.MaxValue
        [Command("roll")]
        [Summary("Rolls a die of user defined size.")]
        public async Task Roll(int size)
            => await (size <= 0
                ? ReplyAsync("That's not how dice work. Try again.")
                : ReplyAsync($"Its {_random.Next(size) + 1}!"));

        // TODO: Allow for more than 2 choices
        [Command("choose")]
        [Summary("Makes a choice between two given options.")]
        public async Task Choose(string choiceOne, string choiceTwo)
        {
            var choice = _random.Next(2) == 0 ? choiceOne : choiceTwo;
            
            await ReplyAsync($"I choose {choice}!");
        }
        
        [Command("flip")]
        [Summary("Flips a coin.")]
        public async Task Flip()
        {
            var choice = _random.Next(2) == 0 ? "heads" : "tails";

            await ReplyAsync($"Its {choice}!");
        }

        [Command("avatar")]
        [Summary("Replies with the user's avatar.")]
        public async Task Avatar(SocketGuildUser user = null)
            => await (user != null
                ? ReplyAsync(user.GetAvatarUrl())
                : ReplyAsync(Context.User.GetAvatarUrl()));

        /*
         * TODO: Finish implementing this
         * Things to consider:
         *  - Calculate votes (this is proving to be tricky!)
         *  - Check who has already voted
         */
        [Command("poll")]
        [Summary("Creates a channel wide polls with a set duration and up to 10 options.")]
        public async Task Poll(string question, int duration, params string[] options)
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

        // TODO: There's probably a better way to store these...
        private readonly List<IEmote> _emoteNumbers = new List<IEmote>
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