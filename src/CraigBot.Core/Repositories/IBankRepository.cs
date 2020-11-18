using System.Threading.Tasks;
using CraigBot.Core.Models;

namespace CraigBot.Core.Repositories
{
    public interface IBankRepository
    {
        Task<Bank> GetByUserId(ulong id);
        
        Task Create(Bank bank);
    }
}