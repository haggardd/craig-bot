namespace CraigBot.Bot.Configuration
{
    public class ModuleFlagOptions
    {
        public const string ConfigurationHeader = "ModuleFlags";
        
        public bool Banking { get; set; }
        
        public bool Betting { get; set; }
        
        public bool Fun { get; set; }
        
        public bool Help { get; set; }
        
        public bool Image { get; set; }
        
        public bool Miscellaneous { get; set; }
        
        public bool Moderation { get; set; }

        public bool Poll { get; set; }
        
        public bool Utility { get; set; }
    }
}