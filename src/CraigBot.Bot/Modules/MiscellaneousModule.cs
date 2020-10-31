using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Summary("Miscellaneous Commands")]
    [RequireContext(ContextType.Guild)]
    public class MiscellaneousModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _random;

        public MiscellaneousModule(Random random)
        {
            _random = random;
        }

        #region Commands
        
        [Command("say")]
        [Summary("Echoes a given piece of text.")]
        [Example("!say \"Welcome new members!\"")]
        [Example("!say \"Welcome new members!\" #general")]
        public async Task Say([Summary("The text you wish to be repeated.")] string text,
            [Summary("The channel you wish message.")] SocketTextChannel channel = null)
            => await (channel == null 
                ? Context.Channel.SendMessageAsync(text)
                : channel.SendMessageAsync(text));
        
        // TODO: Look into preventing numbers over int.MaxValue
        [Command("roll")]
        [Summary("Rolls a 6 sided die or a die of user defined size.")]
        [Example("!roll")]
        [Example("!roll 9")]
        public async Task Roll([Summary("The size of the die you wish to roll.")] 
            int size = 0)
            => await (size <= 0
                ? ReplyAsync($"Its {_random.Next(6) + 1}!")
                : ReplyAsync($"Its {_random.Next(size) + 1}!"));
        
        [Command("choose")]
        [Summary("Makes a choice from a selection of given options.")]
        [Example("!choose cheese bread water")]
        [Example("!choose \"cheese & water\" \"bread & beer\"")]
        public async Task Choose([Summary("The choices you wish the Bot to choose from.")] 
            params string[] choices)
        {
            if (choices.Length <= 1)
            {
                await ReplyAsync("I need at least two choices to make a decision!");
                return;
            }
            
            var randomIndex = _random.Next(choices.Length);
            await ReplyAsync($"I choose {choices[randomIndex]}!");
        }
        
        [Command("flip")]
        [Summary("Flips a coin.")]
        [Example("!flip")]
        public async Task Flip()
        {
            var choice = _random.Next(2) == 0 ? "heads" : "tails";

            await ReplyAsync($"Its {choice}!");
        }

        [Command("avatar")]
        [Summary("Replies with the user's avatar.")]
        [Example("!avatar")]
        [Example("!avatar @Craig")]
        public async Task Avatar([Summary("The user's avatar you wish to see.")] 
            SocketGuildUser user = null)
            => await (user != null
                ? ReplyAsync(user.GetAvatarUrl())
                : ReplyAsync(Context.User.GetAvatarUrl()));
        
        // TODO: Finish implementing this
        /* Things to consider:
        *  - Calculate votes (this is proving to be tricky!)
        *  - Check who has already voted */
        [Command("poll")]
        [Summary("Creates a channel wide poll with a set duration and up to 10 options.")]
        [Example("!poll \"What shall we play?\" 10 \"CS:GO\" \"Red Dead\" \"Sea of Thieves\"")]
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