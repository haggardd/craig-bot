using System;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Core.Repositories;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Fun Commands")]
    public class FunModule : CraigBotBaseModule
    {
        private readonly Random _random;
        private readonly IFortuneRepository _fortuneRepository;
        private readonly IAskResponseRepository _askResponseRepository;

        public FunModule(Random random, IFortuneRepository fortuneRepository,
            IAskResponseRepository askResponseRepository)
        {
            _random = random;
            _fortuneRepository = fortuneRepository;
            _askResponseRepository = askResponseRepository;
        }
        
        #region Commands

        [Command("fortune")]
        [Summary("Replies with a random fortune.")]
        public async Task Fortune()
        {
            var fortunes = (await _fortuneRepository.GetAll()).ToList();
            var randomIndex = _random.Next(fortunes.Count());

            await ReplyAsync(fortunes[randomIndex].Text);
        }

        [Command("ask")]
        [Summary("Replies to a user's question.")]
        [Example("ask Will this losing streak end?!")]
        public async Task Ask([Remainder][Summary("The question you want to ask.")] 
            string question)
        {
            var responses = (await _askResponseRepository.GetAll()).ToList();
            var randomIndex = _random.Next(responses.Count);

            await ReplyAsync(responses[randomIndex].Text);
        }

        #endregion
    }
}