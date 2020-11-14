using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    // TODO: Make sure tests are ran on GitHub actions after pushing to a branch
    public class AudioModuleTests
    {
        [Fact]
        public void AudioModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(AudioModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(AudioModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}