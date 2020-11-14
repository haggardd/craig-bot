using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class HelpModuleTests
    {
        [Fact]
        public void AudioModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(HelpModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(HelpModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}