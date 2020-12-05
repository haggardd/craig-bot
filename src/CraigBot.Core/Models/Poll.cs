using System.Collections.Generic;
using Discord;

namespace CraigBot.Core.Models
{
    public class Poll
    {
        public ulong MessageId { get; set; }
        
        public IChannel Channel { get; set; }

        public string Question { get; set; }

        public Dictionary<int, string> Choices { get; set; }
        
        public Dictionary<ulong, int> Votes { get; set; }

        public bool Ended { get; set; }
    }
}