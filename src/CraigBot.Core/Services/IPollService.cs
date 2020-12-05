using System.Collections.Generic;
using System.Timers;
using CraigBot.Core.Models;
using Discord;

namespace CraigBot.Core.Services
{
    public interface IPollService
    {
        Poll Current { get; set; }
        
        void CreateAndStart(IMessage message, string question, double duration, IEnumerable<string> choices);

        void EndCurrent();

        Dictionary<int, string> CalculateResults();

        void Vote(ulong userId, int choice);

        void OnTimeout(object source, ElapsedEventArgs eventArgs);
    }
}