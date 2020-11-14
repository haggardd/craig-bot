using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class FunModuleTests
    {
        [Fact]
        public void FunModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(FunModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(FunModule).GetCustomAttribute<RequireContextAttribute>(false));
        }
    }
}