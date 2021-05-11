using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class PollHelpers
    {
        public static bool HasUserVoted(this Poll poll, ulong id)
            => poll.Votes.ContainsKey(id);
        
        public static bool IsValidVote(this Poll poll, int choice)
            => poll.Choices.ContainsKey(choice);
        
        public static bool IsActive(this Poll poll)
            => poll is {Ended: false};

        public static string GeneratePercentageBar(int votes, int totalVotes)
        {
            var percentage = (decimal) votes / totalVotes * 100;
            var blockAmount = percentage / 10;
            var bar = "";

            for (var i = 0; i < 10; i++)
            {
                bar += i < blockAmount 
                    ? "█" 
                    : "░";
            }
            
            return bar;
        }
    }
}