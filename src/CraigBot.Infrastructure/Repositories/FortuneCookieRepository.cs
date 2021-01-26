using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CraigBot.Infrastructure.Repositories
{
    public class FortuneCookieRepository : IFortuneCookieRepository
    {
        private readonly CraigBotDbContext _context;
        
        public FortuneCookieRepository(CraigBotDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Fortune>> GetAll()
        {
            var cookies = await _context.FortuneCookies.ToListAsync();

            return cookies;
        }
    }
}