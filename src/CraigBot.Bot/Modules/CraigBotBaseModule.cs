using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class CraigBotBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task InlineReply(IUserMessage userMessage, string messageText)
            => await userMessage.ReplyAsync($"{new Emoji("👉")} {messageText}");
    }
}