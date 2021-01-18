using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class UtilityModuleTests
    {
        [Fact]
        public void UtilityModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(UtilityModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(UtilityModule).GetCustomAttribute<RequireContextAttribute>(true));
        }
    }
}