using System;
using System.Threading.Tasks;
using Discord.Commands;

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
        
        // TODO: Create an overload for taking specific usernames
        [Command("avatar")]
        [Summary("Replies with the user's avatar.")]
        [Remarks("test")]
        public async Task Avatar()
            => await ReplyAsync(Context.User.GetAvatarUrl());

        #endregion
    }
}