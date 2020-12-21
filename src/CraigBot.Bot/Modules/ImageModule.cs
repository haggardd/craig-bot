using System.Threading.Tasks;
using CraigBot.Core.Services;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    // TODO: Finish implementing this
    [Summary("Image Commands")]
    [RequireContext(ContextType.Guild)]
    public class ImageModule : CraigBotBaseModule
    {
        private readonly IImageService _imageService;

        public ImageModule(IImageService imageService)
        {
            _imageService = imageService;
        }
        
        #region Commands

        [Command("Dog")]
        [Summary("Replies with a random dog image.")]
        public async Task Dog()
        {
            var imageUrl = await _imageService.GetRandomDog();
            
            await ReplyAsync(imageUrl);
        }

        [Command("Cat")]
        [Summary("Replies with a random cat image.")]
        public async Task Cat()
            => await ReplyAsync("Not implemented!");

        #endregion
    }
}