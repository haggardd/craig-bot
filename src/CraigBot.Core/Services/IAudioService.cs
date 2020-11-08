using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;

namespace CraigBot.Core.Services
{
    public interface IAudioService
    {
        IAudioClient AudioClient { get; set; }
        
        Task OnConnectToVoiceChannel(SocketVoiceServer voiceChannel);
    }
}