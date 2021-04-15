namespace CraigBot.Core.Models
{
    public class Investment
    {
        public int Id { get; set; }
        
        public ulong UserId { get; set; }
        
        public int StockId { get; set; }
        
        public int Amount { get; set; }
        
        public decimal BuyPrice { get; set; }
    }
}