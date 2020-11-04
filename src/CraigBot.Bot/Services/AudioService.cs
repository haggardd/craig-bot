using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;

namespace CraigBot.Bot.Services
{
    public class AudioService
    {
        public IAudioClient AudioClient { get; set; }
        
        private readonly DiscordSocketClient _discord;

        public AudioService(DiscordSocketClient discord)
        {
            _discord = discord;

            _discord.VoiceServerUpdated += OnConnectToVoiceChannel;
        }

        private async Task OnConnectToVoiceChannel(SocketVoiceServer voiceChannel)
        {
            var botUser = _discord.GetGuild(voiceChannel.Guild.Id).GetUser(_discord.CurrentUser.Id);
            
            if (!botUser.IsDeafened)
            {
                await botUser.ModifyAsync(x =>
                {
                    x.Deaf = true;
                });
            }
        }
    }
}