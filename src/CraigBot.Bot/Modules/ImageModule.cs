using System.Threading.Tasks;
using CraigBot.Core.Services;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Image Commands")]
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
            
            if (imageUrl == null)
            {
                await ReplyAsync("The API failed to return a dog.");
            }
            
            await ReplyAsync(imageUrl);
        }

        [Command("Cat")]
        [Summary("Replies with a random cat image.")]
        public async Task Cat()
        {
            var imageUrl = await _imageService.GetRandomCat();

            if (imageUrl == null)
            {
                await ReplyAsync("The API failed to return a cat.");
            }
            
            await ReplyAsync(imageUrl);
        }
        
        [Command("Fox")]
        [Summary("Replies with a random fox image.")]
        public async Task Fox()
        {
            var imageUrl = await _imageService.GetRandomFox();
            
            if (imageUrl == null)
            {
                await ReplyAsync("The API failed to return a fox.");
            }
            
            await ReplyAsync(imageUrl);
        }

        #endregion
    }
}