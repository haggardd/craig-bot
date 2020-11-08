using System.Threading.Tasks;
using Discord;

namespace CraigBot.Core.Services
{
    public interface ILoggingService
    {
        Task OnLog(LogMessage message);
    }
}