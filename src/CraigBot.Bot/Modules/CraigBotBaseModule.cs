using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class CraigBotBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task AddReactionAndMentionReply(string messageText, IUserMessage userMessage, IEmote emote,
            EmbedBuilder embed = null, ResponseTypes responseType = ResponseTypes.None)
        {
            await userMessage.AddReactionAsync(emote);

            await (embed == null
                ? ReplyAsync($"{ResponseTypeToEmojiUnicode(responseType)} {Context.User.Mention} -> {messageText}")
                : ReplyAsync($"{ResponseTypeToEmojiUnicode(responseType)} {Context.User.Mention} -> {messageText}", false, embed.Build()));
        }

        protected async Task ReplyAndAddReactions(string messageText, IEmote[] emotes, EmbedBuilder embed = null)
        {
            var message = embed == null
                ? await ReplyAsync(messageText)
                : await ReplyAsync(messageText, false, embed.Build());

            await message.AddReactionsAsync(emotes);
        }
        
        protected async Task MentionReply(string messageText, ResponseTypes responseType = ResponseTypes.None)
            => await ReplyAsync($"{ResponseTypeToEmojiUnicode(responseType)} {Context.User.Mention} -> {messageText}");
        
        private Emoji ResponseTypeToEmojiUnicode(ResponseTypes responseType)
        {
            var emojiCode = responseType switch
            {
                ResponseTypes.None => "",
                ResponseTypes.Information => "ℹ",
                ResponseTypes.Warning => "⚠",
                ResponseTypes.Error => "‼",
                _ => ""
            };

            return new Emoji(emojiCode);
        }
    }
}