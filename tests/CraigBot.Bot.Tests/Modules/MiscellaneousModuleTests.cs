using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class MiscellaneousModuleTests
    {
        [Fact]
        public void MiscellaneousModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(MiscellaneousModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(MiscellaneousModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}