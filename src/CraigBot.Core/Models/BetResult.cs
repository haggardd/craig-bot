using System.Collections.Generic;

namespace CraigBot.Core.Models
{
    public class BetResult
    {
        public string Username { get; set; }
        
        public string Description { get; set; }
        
        public IEnumerable<WagerResult> WagerResults { get; set; }
    }
}