using Newtonsoft.Json;

namespace CraigBot.Bot.Services.ApiResponses
{
    public class CatResponse
    {
        [JsonProperty("file")]
        public string ImageUrl { get; set; }
    }
}