using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class PollModuleTests
    {
        [Fact]
        public void PollModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(PollModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(PollModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}