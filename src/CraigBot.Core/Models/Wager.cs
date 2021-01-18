namespace CraigBot.Core.Models
{
    public class Wager
    {
        public int Id { get; set; }

        public int BetId { get; set; }

        public ulong UserId { get; set; }
        
        public string Username { get; set; }
        
        public decimal Stake { get; set; }
        
        public bool InFavour { get; set; }
    }
}