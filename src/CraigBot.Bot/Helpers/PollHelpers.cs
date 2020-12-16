using CraigBot.Core.Models;

namespace CraigBot.Bot.Helpers
{
    public static class PollHelpers
    {
        public static bool HasUserVoted(this Poll poll, ulong id)
            => poll.Votes.ContainsKey(id);
        
        public static bool IsValidVote(this Poll poll, int choice)
            => poll.Choices.ContainsKey(choice);
        
        public static bool IsPollActive(this Poll poll)
            => poll != null && !poll.Ended;
    }
}