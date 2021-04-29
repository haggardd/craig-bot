using System;

namespace CraigBot.Core.Models
{
    public class Stock
    {
        public int Id { get; set; }
        
        public string Ticker { get; set; }
        
        public decimal Price { get; set; }
        
        public decimal High { get; set; }
        
        public decimal Low { get; set; }
        
        public decimal PreviousPrice { get; set; }
        
        // TODO: This could be moved to a config as a single value, read on startup and updated when needed
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0#options-interfaces
        public DateTime LastUpdate { get; set; }
    }
}