using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
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