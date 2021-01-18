using System.Reflection;
using CraigBot.Bot.Modules;
using Discord.Commands;
using Xunit;

namespace CraigBot.Bot.Tests.Modules
{
    public class BankingModuleTests
    {
        [Fact]
        public void BankingModule_HasCorrectAttributes()
        {
            Assert.NotNull(typeof(BankingModule).GetCustomAttribute<SummaryAttribute>(false));
            Assert.NotNull(typeof(BankingModule).GetCustomAttribute<RequireContextAttribute>(true));
        }
    }
}