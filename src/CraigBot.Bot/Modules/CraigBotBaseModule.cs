using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    public abstract class CraigBotBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task ReplyAndAddReactionAsync(string messageText, IUserMessage userMessage, IEmote emote)
        {
            await userMessage.AddReactionAsync(emote);
            await ReplyAsync(messageText);
        }
    }
}