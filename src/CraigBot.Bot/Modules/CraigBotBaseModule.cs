using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class CraigBotBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task AddReactionAndMentionReply(string messageText, IUserMessage userMessage, IEmote emote, 
            EmbedBuilder embed = null)
        {
            await userMessage.AddReactionAsync(emote);

            await (embed == null
                ? ReplyAsync($"{Context.User.Mention} -> {messageText}")
                : ReplyAsync($"{Context.User.Mention} -> {messageText}", false, embed.Build()));
        }

        protected async Task ReplyAndAddReactions(string messageText, IEmote[] emotes, EmbedBuilder embed = null)
        {
            var message = embed == null
                ? await ReplyAsync(messageText)
                : await ReplyAsync(messageText, false, embed.Build());

            await message.AddReactionsAsync(emotes);
        }
        
        protected async Task MentionReply(string messageText)
            => await ReplyAsync($"{Context.User.Mention} -> {messageText}");
    }
}