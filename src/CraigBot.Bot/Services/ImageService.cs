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
            var response = await _httpClient.GetStringAsync(_options.DogApiUrl);

            if (response == null)
            {
                return null;
            }
            
            var dog = JsonConvert.DeserializeObject<DogResponse>(response);

            return dog.ImageUrl;
        }

        public async Task<string> GetRandomCat()
        {
            var response = await _httpClient.GetStringAsync(_options.CatApiUrl);
            
            if (response == null)
            {
                return null;
            }

            var cat = JsonConvert.DeserializeObject<CatResponse>(response);

            return cat.ImageUrl;
        }

        public async Task<string> GetRandomFox()
        {
            var response = await _httpClient.GetStringAsync(_options.FoxApiUrl);
            
            if (response == null)
            {
                return null;
            }

            var fox = JsonConvert.DeserializeObject<FoxResponse>(response);

            return fox.ImageUrl;
        }
    }
}