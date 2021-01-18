using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class ModerationModuleTests
    {
        [Fact]
        public void ModerationModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(ModerationModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(ModerationModule).GetCustomAttribute<RequireContextAttribute>(true));
        }
    }
}