namespace CraigBot.Core.Models
{
    public class Bet
    {
        public int Id { get; set; }

        public ulong UserId { get; set; }
        
        public string Username { get; set; }

        public string Description { get; set; }

        public string ForOdds { get; set; }
        
        public string AgainstOdds { get; set; }
        
        public bool HasEnded { get; set; }
    }
}