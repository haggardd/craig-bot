using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Services;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Poll Commands")]
    [RequireContext(ContextType.Guild)]
    public class PollModule : CraigBotBaseModule
    {
        private readonly IPollService _pollService;

        public PollModule(IPollService pollService)
        {
            _pollService = pollService;
        }
        
        #region Commands
        
        [Command("poll")]
        [Summary("Creates a poll with a set duration and multiple choices.")]
        [Example("poll \"What shall we play?\" 10 \"CS:GO\" \"Red Dead\" \"Sea of Thieves\"")]
        public async Task Poll([Summary("The question you'd like to propose in the poll.")] string question, 
            [Summary("The amount of seconds to pass before the winner is decided.")] int duration, 
            [Summary("The choices to pick from during the poll.")] params string[] choices)
        {
            if (_pollService.Current.IsPollActive())
            {
                await ReplyAsync("There is already an active poll, either end the current poll or wait for it to end.");
                return;
            }
            
            if (choices.Length < 2)
            {
                await ReplyAsync("To start a poll you need at least 2 choices to choose from.");
                return;
            }

            var choicesText = "";
            
            for (var i = 0; i < choices.Length; i++)
            {
                choicesText += $"`{i + 1})` {choices[i]} \n";
            }
            
            var pollEmbed = BasePollEmbed()
                .WithTitle(question)
                .WithDescription("Use `!vote` with choice number to cast your vote!")
                .WithFooter(f => f.Text = $"Poll ends {duration} seconds from message sent")
                .AddField("Choices: ", choicesText);

            await ReplyAsync("", false, pollEmbed.Build());
            
            _pollService.Create(question, choices);
            
            await Task.Delay(duration * 1000);
            
            await EndPoll();
        }

        [Command("vote")]
        [Summary("Casts a vote during a poll.")]
        [Example("vote 1")]
        public async Task Vote([Summary("The choice you wish to vote for.")] int choice)
        {
            var userId = Context.User.Id;
            
            // TODO: Need to have a think about how error handling is current handled in modules, might be better to move some to the services
            if (!_pollService.Current.IsPollActive())
            {
                await ReplyAndAddReactionAsync("There are no active polls.", 
                    Context.Message, _invalidEmoji);
                return;
            }

            if (_pollService.Current.HasUserVoted(userId))
            {
                await ReplyAndAddReactionAsync("You've already voted in the current poll.", 
                    Context.Message, _invalidEmoji);
                return;
            }

            if (!_pollService.Current.IsValidVote(choice))
            {
                await ReplyAndAddReactionAsync("Invalid choice. Please vote for one of the choices in the poll message.", 
                    Context.Message, _invalidEmoji);
                return;
            }

            _pollService.Vote(userId, choice);
            
            await Context.Message.AddReactionAsync(_tickEmoji);
        }
        
        [Command("end")]
        [Summary("Ends the current poll.")]
        public async Task End()
        {
            if (!_pollService.Current.IsPollActive())
            {
                await ReplyAsync("There are no active polls.");
                return;
            }

            await EndPoll();
        }
        
        #endregion

        #region Helpers

        private async Task EndPoll()
        {
            if (!_pollService.Current.IsPollActive())
            {
                return;
            }
            
            var results = _pollService.CalculateResults();
            
            var resultsText = "";
            
            if (results.Any())
            {
                for (var i = 0; i < results.Count; i++)
                {
                    var key = results.ElementAt(i).Key;
                    resultsText += $"• {_pollService.Current.Choices[key]} -- `{results.ElementAt(i).Value}`\n";
                }
            }
            else
            {
                resultsText = "No votes were cast!";
            }

            var pollOverEmbed = BasePollEmbed()
                .WithTitle("Poll over!")
                .WithDescription(_pollService.Current.Question)
                .AddField("Vote Results", resultsText);

            await ReplyAsync("", false, pollOverEmbed.Build());
            
            _pollService.EndCurrent();
        }
        
        private EmbedBuilder BasePollEmbed()
            => new EmbedBuilder()
                .WithColor(Color.Gold)
                .WithAuthor(Context.User.Username, Context.User.GetAvatarUrl());
        
        private readonly Emoji _tickEmoji = new Emoji("\u2705");
        
        private readonly Emoji _invalidEmoji = new Emoji("\uD83D\uDEAB");

        #endregion
    }
}