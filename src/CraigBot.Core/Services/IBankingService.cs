using System.Threading.Tasks;
using Discord.WebSocket;

namespace CraigBot.Core.Services
{
    public interface IBankingService
    {
        Task Create(SocketUser user);
    }
}