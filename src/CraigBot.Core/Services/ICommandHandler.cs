using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CraigBot.Core.Services
{
    public interface ICommandHandler
    {
        Task OnMessageReceived(SocketMessage socketMessage);

        Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result);
    }
}