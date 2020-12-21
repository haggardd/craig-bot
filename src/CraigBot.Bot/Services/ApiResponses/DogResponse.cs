using Newtonsoft.Json;

namespace CraigBot.Bot.Services.ApiResponses
{
    public class DogResponse
    { 
        [JsonProperty("url")]
        public string ImageUrl { get; set; }
    }
}