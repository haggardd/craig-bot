using System.Net.Http;
using System.Threading.Tasks;
using CraigBot.Bot.Configuration;
using CraigBot.Bot.Services.ApiResponses;
using CraigBot.Core.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CraigBot.Bot.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiOptions _options;

        public ImageService(HttpClient httpClient, IOptions<ApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        
        public async Task<string> GetRandomDog()
        {
            // TODO: Need to have a think about error handle for API calls
            var response = await _httpClient.GetStringAsync(_options.DogApiUrl); 

            var dog = JsonConvert.DeserializeObject<DogResponse>(response);

            return dog.ImageUrl;
        }
    }
}