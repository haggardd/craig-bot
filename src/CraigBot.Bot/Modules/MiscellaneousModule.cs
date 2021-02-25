using System;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Common;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Bot.Modules
{
    [Summary("Miscellaneous Commands")]
    public class MiscellaneousModule : CraigBotBaseModule
    {
        private readonly Random _random;

        public MiscellaneousModule(Random random)
        {
            _random = random;
        }

        #region Commands
        
        [Command("say")]
        [Summary("Echoes a given piece of text.")]
        [Example("say \"Welcome new members!\"")]
        [Example("say \"Welcome new members!\" #general")]
        public async Task Say([Summary("The text you wish to be repeated.")] string text,
            [Summary("The channel you wish message.")] SocketTextChannel channel = null)
            => await (channel == null 
                ? Context.Channel.SendMessageAsync(text)
                : channel.SendMessageAsync(text));
        
        [Command("roll")]
        [Summary("Rolls a 6 sided die or a die of user defined size.")]
        [Example("roll")]
        [Example("roll 9")]
        public async Task Roll([Summary("The size of the die you wish to roll.")] 
            int size = 0)
            => await (size <= 0
                ? ReplyAsync($"Its {_random.Next(6) + 1}!")
                : ReplyAsync($"Its {_random.Next(size) + 1}!"));
        
        [Command("choose")]
        [Summary("Makes a choice from a selection of given options.")]
        [Example("choose cheese bread water")]
        [Example("choose \"cheese & water\" \"bread & beer\"")]
        public async Task Choose([Summary("The choices you wish the Bot to choose from.")] 
            params string[] choices)
        {
            if (choices.Length <= 1)
            {
                await MentionReply("I need at least two choices to make a decision.", ResponseTypes.Information);
                return;
            }
            
            var randomIndex = _random.Next(choices.Length);
            await ReplyAsync($"I choose {choices[randomIndex]}!");
        }
        
        [Command("flip")]
        [Summary("Flips a coin.")]
        public async Task Flip()
        {
            var result = _random.Next(2) == 0 ? "heads" : "tails";

            await ReplyAsync($"Its {result}!");
        }

        [Command("avatar")]
        [Summary("Replies with the user's avatar.")]
        [Example("avatar")]
        [Example("avatar @Craig")]
        public async Task Avatar([Summary("The user's avatar you wish to see.")] 
            SocketGuildUser user = null)
            => await (user != null
                ? ReplyAsync(user.GetAvatarUrl())
                : ReplyAsync(Context.User.GetAvatarUrl()));

        #endregion
    }
}