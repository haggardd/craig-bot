using System.Threading.Tasks;

namespace CraigBot.Core.Services
{
    public interface IStartupService
    {
        Task StartAsync();
    }
}