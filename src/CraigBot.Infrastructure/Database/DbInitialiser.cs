using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using Newtonsoft.Json;

namespace CraigBot.Infrastructure.Database
{
    public static class DbInitialiser
    {
        public static async Task Initialise(CraigBotDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
                
            if (!context.Fortunes.Any())
            {
                var parsedFortunes = await DeserializeJsonCollection<string>("fortunes");
                var fortunes = parsedFortunes.Select(x => new Fortune {Text = x});
                    
                await context.Fortunes.AddRangeAsync(fortunes);
            }

            if (!context.AskResponses.Any())
            {
                var parsedResponses = await DeserializeJsonCollection<string>("askResponses");
                var responses = parsedResponses.Select(x => new AskResponse {Text = x});
                    
                await context.AskResponses.AddRangeAsync(responses);
            }
            
            if (!context.Stocks.Any())
            {
                var stocks = GenerateStocks();

                await context.Stocks.AddRangeAsync(stocks);
            }
                
            await context.SaveChangesAsync();
        }

        private static async Task<IEnumerable<T>> DeserializeJsonCollection<T>(string fileName)
        {
            var json = await File.ReadAllTextAsync($"Database/Data/{fileName}.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return data;
        }

        private static IEnumerable<Stock> GenerateStocks()
        {
            const int amount = 10;
            const int high = 300;
            const int low = 10;

            var rng = new Random();
            var stocks = new List<Stock>();
            var updateTime = DateTime.Now;

            for (var i = 0; i < amount; i++)
            {
                var price = rng.Next(low, high);
                var ticker = GenerateRandomTicker(rng);
                
                var stock = new Stock
                {
                    Ticker = ticker,
                    Price = price,
                    High = price,
                    Low = price,
                    PreviousPrice = 0,
                    LastUpdate = updateTime
                };
                
                stocks.Add(stock);
            }

            return stocks;
        }

        private static string GenerateRandomTicker(Random rng)
        {
            const int length = 3;
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            var result = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                result.Append(characters[rng.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}