using System.Collections.Generic;
using System.Linq;
using CraigBot.Core.Models;
using CraigBot.Core.Services;

namespace CraigBot.Bot.Services
{
    public class PollService : IPollService
    {
        public Poll Current { get; set; }

        public void Create(string question, IEnumerable<string> choices)
        {
            var choicesDictionary = choices
                .Select((value, index) => new { v = value, i = index + 1 })
                .ToDictionary(x => x.i, x => x.v);
            
            var newPoll = new Poll
            {
                Question = question,
                Choices = choicesDictionary,
                Votes = new Dictionary<ulong, int>(),
                Ended = false
            };

            Current = newPoll;
        }

        public void EndCurrent()
        {
            Current.Ended = true;
        }
        
        public Dictionary<int, int> CalculateResults()
        {
            var results = Current.Votes.Values
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            return results;
        }

        public void Vote(ulong userId, int choice)
        {
            Current.Votes.Add(userId, choice);
        }
    }
}