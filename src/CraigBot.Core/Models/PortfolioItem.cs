namespace CraigBot.Core.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        
        public string StockTicker { get; set; }
        
        public int Amount { get; set; }
        
        public decimal BuyPrice { get; set; }
        
        public decimal CurrentPrice { get; set; }
    }
}