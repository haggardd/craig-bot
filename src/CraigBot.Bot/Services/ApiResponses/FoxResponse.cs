using Newtonsoft.Json;

namespace CraigBot.Bot.Services.ApiResponses
{
    public class FoxResponse
    {
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
    }
}