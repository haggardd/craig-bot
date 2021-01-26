using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                
            if (!context.FortuneCookies.Any())
            {
                var parsedFortunes = await DeserializeJsonCollection<string>("fortunes");
                var fortunes = parsedFortunes.Select(x => new Fortune {Text = x});
                    
                await context.FortuneCookies.AddRangeAsync(fortunes);
            }

            if (!context.EightBallResponses.Any())
            {
                var parsedResponses = await DeserializeJsonCollection<string>("eightBallResponses");
                var responses = parsedResponses.Select(x => new EightBallResponse {Text = x});
                    
                await context.EightBallResponses.AddRangeAsync(responses);
            }
                
            await context.SaveChangesAsync();
        }

        private static async Task<IEnumerable<T>> DeserializeJsonCollection<T>(string fileName)
        {
            var json = await File.ReadAllTextAsync($"Database/Data/{fileName}.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return data;
        }
    }
}