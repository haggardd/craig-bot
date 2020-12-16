using System.Collections.Generic;
using CraigBot.Core.Models;

namespace CraigBot.Core.Services
{
    public interface IPollService
    {
        Poll Current { get; set; }
        
        void Create(string question, IEnumerable<string> choices);

        void EndCurrent();

        Dictionary<int, int> CalculateResults();

        void Vote(ulong userId, int choice);
    }
}