using System.Threading.Tasks;
using CraigBot.Bot.Common;
using CraigBot.Bot.Services;
using Discord;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    /* TODO: Finish implementing this
     * Things to consider:
     * - Get basic YouTube playback working first
     * - Might want to limit the size of the queue */
    [Summary("Audio Commands")]
    [RequireContext(ContextType.Guild)]
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _audioService;

        public AudioModule(AudioService audioService)
        {
            _audioService = audioService;
        }
        
        [Command("join", RunMode = RunMode.Async)]
        [Summary("Joins your current voice channel.")]
        public async Task Join()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await ReplyAsync("You have to be in a voice channel for me to join!");
                return;
            }
            
            _audioService.AudioClient = await channel.ConnectAsync();
        }
        
        [Command("leave")]
        [Summary("Leaves its current voice channel.")]
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
        
        [Command("play")]
        [Summary("Plays a video via voice chat.")]
        [Example("play https://www.youtube.com/watch?v=j_ekugPKqFw")]
        public async Task Play([Remainder][Summary("A URL for the YouTube video you wish to play.")] string url)
        {
            await ReplyAsync("Not implemented yet...");
        }
        
        [Command("skip")]
        [Summary("Skips the current video and plays the next one in the queue. If it's the last one, playback will stop completely.")]
        public async Task Skip()
        {
            await ReplyAsync("Not implemented yet...");
        }
        
        [Command("queue")]
        [Summary("Presents the current video queue.")]
        public async Task Queue()
        {
            await ReplyAsync("Not implemented yet...");
        }
    }
}