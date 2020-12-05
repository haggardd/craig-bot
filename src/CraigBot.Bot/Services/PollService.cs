using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CraigBot.Core.Models;
using CraigBot.Core.Services;
using Discord;

namespace CraigBot.Bot.Services
{
    public class PollService : IPollService
    {
        public Poll Current { get; set; }
        
        private readonly Timer _timer;

        public PollService()
        {
            _timer = new Timer();

            _timer.Elapsed += OnTimeout;
            _timer.AutoReset = false;
        }

        public void CreateAndStart(IMessage message, string question, double duration, IEnumerable<string> choices)
        {
            var choicesDictionary = choices
                .Select((value, index) => new { v = value, i = index + 1 })
                .ToDictionary(x => x.i, x => x.v);
            
            var newPoll = new Poll
            {
                MessageId = message.Id,
                Channel = message.Channel,
                Question = question,
                Choices = choicesDictionary,
                Votes = new Dictionary<ulong, int>(),
                Ended = false
            };

            Current = newPoll;

            _timer.Interval = duration * 1000;
            _timer.Start();
        }

        public void EndCurrent()
        {    
            _timer.Stop();
            Current.Ended = true;
        }
        
        public Dictionary<int, string> CalculateResults()
        {
            // TODO: This is pretty messy...
            var results = Current.Votes.Values
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            var mostVotes = results
                .Select(x => x.Value)
                .Where(x => x >= results.Max().Value)
                .ToList();

            var winners = Current.Choices
                .Select(x => x)
                .Where(x => mostVotes.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            return winners;
        }

        public void Vote(ulong userId, int choice)
        {
            Current.Votes.Add(userId, choice);
        }

        public void OnTimeout(object source, ElapsedEventArgs eventArgs)
        {
            EndCurrent();
        }
    }
}