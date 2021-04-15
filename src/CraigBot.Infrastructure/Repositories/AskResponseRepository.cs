using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class AskResponseRepository : IAskResponseRepository
    {
        public async Task<IEnumerable<AskResponse>> GetAll()
        {
            await using var context = new CraigBotDbContext();
            
            var responses = await context.AskResponses.ToListAsync();

            return responses;
        }
    }
}