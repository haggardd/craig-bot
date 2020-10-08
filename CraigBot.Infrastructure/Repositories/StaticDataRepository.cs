using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CraigBot.Domain.Repositories;
using Newtonsoft.Json;

namespace CraigBot.Infrastructure.Repositories
{
    public class StaticDataRepository : IStaticDataRepository
    {
        public async Task<IEnumerable<string>> Get(string fileName)
        {
            var json = await File.ReadAllTextAsync($"Repositories/Data/{fileName}.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<string>>(json);

            return data;
        }
    }
}