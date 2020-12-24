namespace CraigBot.Bot.Configuration
{
    public class ApiOptions
    {
        public const string ConfigurationHeader = "Apis";
        
        public string DogApiUrl { get; set; }
        
        public string CatApiUrl { get; set; }
        
        public string FoxApiUrl { get; set; }
    }
}