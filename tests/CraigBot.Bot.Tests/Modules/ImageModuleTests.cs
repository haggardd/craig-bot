using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class ImageModuleTests
    {
        [Fact]
        public void ImageModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(ImageModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(ImageModule).GetCustomAttribute<RequireContextAttribute>(true));
        }
    }
}