using System.Collections.Generic;
using System.Threading.Tasks;

namespace CraigBot.Domain.Repositories
{
    public interface IStaticDataRepository
    {
        Task<IEnumerable<string>> Get(string fileName);
    }
}