using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

#pragma warning disable 1998
namespace CraigBot.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PreventSelfMentionAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var userId = context.User.Id;
            var mentionedUserIds = context.Message.MentionedUserIds;

            return mentionedUserIds.Contains(userId)
                ? PreconditionResult.FromError("Command can't contain a self mention")
                : PreconditionResult.FromSuccess();
        }
    }
}