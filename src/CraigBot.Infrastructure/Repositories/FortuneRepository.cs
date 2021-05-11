using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Repositories
{
    public class FortuneRepository : IFortuneRepository
    {
        // TODO: Could probably cache these on first fetch
        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0
        public async Task<IEnumerable<Fortune>> GetAll()
        {
            await using var context = new CraigBotDbContext();
            
            var cookies = await context.Fortunes.ToListAsync();

            return cookies;
        }
    }
}