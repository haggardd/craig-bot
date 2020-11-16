using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;

namespace CraigBot.Infrastructure.Repositories
{
    public class EightBallResponseRepository : IEightBallResponseRepository
    {
        private readonly CraigBotDbContext _context;
        
        public EightBallResponseRepository(CraigBotDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<EightBallResponse>> GetAll()
        {
            var responses = await _context.EightBallResponses.ToListAsync();

            return responses;
        }
    }
}