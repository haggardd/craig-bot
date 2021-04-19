using CraigBot.Core.Models;

namespace CraigBot.Core.Mappers
{
    public static class InvestmentMappers
    {
        public static PortfolioItem ToPortfolioItem(this Investment investment, Stock stock)
        {
            return new PortfolioItem
            {
                Id = investment.Id,
                StockTicker = stock.Ticker,
                Amount = investment.Amount,
                BuyPrice = investment.BuyPrice,
                CurrentPrice = stock.Price
            };
        }
    }
}