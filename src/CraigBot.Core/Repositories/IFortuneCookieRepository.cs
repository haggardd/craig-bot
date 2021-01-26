using System.Collections.Generic;
using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IFortuneCookieRepository
    {
        Task<IEnumerable<Fortune>> GetAll();
    }
}