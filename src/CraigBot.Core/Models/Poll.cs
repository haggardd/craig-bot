using System.Collections.Generic;

namespace CraigBot.Core.Models
{
    public class Poll
    {
        public string Question { get; set; }

        public Dictionary<int, string> Choices { get; set; }
        
        public Dictionary<ulong, int> Votes { get; set; }

        public bool Ended { get; set; }
    }
}