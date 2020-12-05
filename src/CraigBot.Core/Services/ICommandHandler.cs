using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CraigBot.Core.Services
{
    public interface ICommandHandler
    {
        Task OnMessageReceived(IMessage message);

        Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result);
    }
}