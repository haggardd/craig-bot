using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

#pragma warning disable 1998
namespace CraigBot.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PreventCraigMentionAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var craigId = context.Client.CurrentUser.Id;
            var mentionedUserIds = context.Message.MentionedUserIds;

            return mentionedUserIds.Contains(craigId)
                ? PreconditionResult.FromError("Command can't contain Craig mention")
                : PreconditionResult.FromSuccess();
        }
    }
}