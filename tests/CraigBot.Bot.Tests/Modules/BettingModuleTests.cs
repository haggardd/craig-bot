using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class BettingModuleTests
    {
        [Fact]
        public void BettingModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(BettingModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(BettingModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}