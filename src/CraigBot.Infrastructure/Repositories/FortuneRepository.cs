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
        // TODO: Could probably store these on first fetch
        public async Task<IEnumerable<Fortune>> GetAll()
        {
            await using var context = new CraigBotDbContext();
            
            var cookies = await context.Fortunes.ToListAsync();

            return cookies;
        }
    }
}