using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Services
{
    public class InvestingService : IInvestingService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly BotOptions _options;
        
        public InvestingService(IInvestmentRepository investmentRepository, IStockRepository stockRepository,
            IOptions<BotOptions> options)
        {
            _investmentRepository = investmentRepository;
            _stockRepository = stockRepository;
            _options = options.Value;
        }

        public async Task<IEnumerable<Stock>> GetAllStocks()
        {
            var stocks = (await _stockRepository.GetAll()).ToList();

            if (stocks.First().IsOutOfDate(_options.MarketUpdateRate))
            {
                stocks = await UpdateStockMarket();
            }
            
            var orderedStocks = stocks.OrderByDescending(x => x.Price).ToList();
            
            return orderedStocks;
        }

        public async Task<Investment> GetInvestmentById(int id)
        {
            var investment = await _investmentRepository.GetByInvestmentId(id);

            return investment;
        }

        public async Task<IEnumerable<PortfolioItem>> GetPortfolioByUserId(ulong id)
        {
            var investments  = (await _investmentRepository.GetAllByUserId(id)).ToList();

            if (!investments.Any())
            {
                return null;
            }

            var stockIds = investments.Select(x => x.StockId).ToList();
            var stocks = (await _stockRepository.GetAllByIds(stockIds)).ToList();

            if (stocks.First().IsOutOfDate(_options.MarketUpdateRate))
            {
                var updatedStocks = await UpdateStockMarket();
                stocks = updatedStocks.Where(x => stockIds.Contains(x.Id)).ToList();
            }

            var portfolio = new List<PortfolioItem>();
            
            foreach (var investment in investments)
            {
                var stock = stocks.First(x => x.Id == investment.StockId);
                
                // TODO: Might be a good idea to make some mappers for this stuff
                var portfolioItem = new PortfolioItem
                {
                    Id = investment.Id,
                    StockTicker = stock.Ticker,
                    Amount = investment.Amount,
                    BuyPrice = investment.BuyPrice,
                    CurrentPrice = stock.Price
                };
                
                portfolio.Add(portfolioItem);
            }
            
            return portfolio;
        }

        public async Task<Stock> GetStockById(int id)
        {
            var stock = await _stockRepository.GetById(id);
            
            if (!stock.IsOutOfDate(_options.MarketUpdateRate))
            {
                return stock;
            }

            var stocks = await UpdateStockMarket();
            
            stock = stocks.FirstOrDefault(x => x.Id == id);

            return stock;
        }

        public async Task<Stock> GetStockByTicker(string ticker)
        {
            var stock = await _stockRepository.GetByTicker(ticker);

            if (!stock.IsOutOfDate(_options.MarketUpdateRate))
            {
                return stock;
            }

            var stocks = await UpdateStockMarket();
            
            stock = stocks.FirstOrDefault(x => x.Ticker == ticker);
            
            return stock;
        }

        public async Task<Investment> CreateInvestment(ulong userId, int stockId, int amount, decimal buyPrice)
        {
            var investment = new Investment
            {
                UserId = userId,
                StockId = stockId,
                Amount = amount,
                BuyPrice = buyPrice
            };

            var newInvestment = await _investmentRepository.Create(investment);

            return newInvestment;
        }

        public async Task<Investment> UpdateInvestment(Investment investment)
        {
            var updatedInvestment = await _investmentRepository.Update(investment);

            return updatedInvestment;
        }

        public async Task DeleteInvestment(Investment investment)
        {
            await _investmentRepository.Delete(investment);
        }

        private async Task<List<Stock>> UpdateStockMarket()
        {
            var stocks = (await _stockRepository.GetAll()).ToList();
            
            var updateTime = DateTime.Now;
                    
            foreach (var stock in stocks)
            {
                stock.LastUpdate = updateTime;
                stock.PreviousPrice = stock.Price;
                stock.Price = stock.CalculateNextPrice();
                    
                if (stock.Price > stock.High)
                {
                    stock.High = stock.Price;
                } 
                else if (stock.Price < stock.Low)
                {
                    stock.Low = stock.Price;
                }
            }
            
            await _stockRepository.UpdateAll(stocks);

            return stocks;
        }
    }
}