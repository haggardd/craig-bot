﻿namespace CraigBot.Bot.Configuration
{
    public class BotOptions
    {
        public const string ConfigurationHeader = "Bot";
        
        public string Token { get; set; }
        
        public string Prefix { get; set; }

        public bool DmHelp { get; set; }
        
        public decimal MessageReward { get; set; }
        
        public decimal StartingBalance { get; set; }
        
        public char Currency { get; set; }
        
        public int MarketUpdateRate { get; set; }
    }
}