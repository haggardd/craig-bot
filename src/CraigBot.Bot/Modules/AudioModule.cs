using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    
    [Summary("Audio Commands")]
    [RequireContext(ContextType.Guild)]
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        [Command("join", RunMode = RunMode.Async)]
        [Summary("Makes the Bot join your current voice channel.")]
        [Example("!join")]
        public async Task Join()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await ReplyAsync("You have to be in a voice channel for me to join!");
                return;
            }
            
            await channel.ConnectAsync();
        }
        
        [Command("leave")]
        [Summary("Makes the Bot leave its current voice channel.")]
        [Example("!leave")]
        public async Task Leave()
        {
            var bot = Context.Guild.GetUser(Context.Client.CurrentUser.Id);
            var channel = bot.VoiceChannel;

            if (channel == null)
            {
                await ReplyAsync("I'm not currently in a voice channel.");
                return;
            }
            
            await channel.DisconnectAsync();
        }
    }
}