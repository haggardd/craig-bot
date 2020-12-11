using System;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Core.Repositories;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Fun Commands")]
    [RequireContext(ContextType.Guild)]
    public class FunModule : CraigBotBaseModule
    {
        private readonly Random _random;
        private readonly IFortuneCookieRepository _fortuneCookieRepository;
        private readonly IEightBallResponseRepository _eightBallResponseRepository;

        public FunModule(Random random, IFortuneCookieRepository fortuneCookieRepository,
            IEightBallResponseRepository eightBallResponseRepository)
        {
            _random = random;
            _fortuneCookieRepository = fortuneCookieRepository;
            _eightBallResponseRepository = eightBallResponseRepository;
        }
        
        #region Commands

        [Command("fortune")]
        [Summary("Replies with a random fortune.")]
        public async Task Fortune()
        {
            var fortunes = (await _fortuneCookieRepository.GetAll()).ToList();
            var randomIndex = _random.Next(fortunes.Count());

            await ReplyAsync(fortunes[randomIndex].Fortune);
        }

        [Command("8ball")]
        [Summary("Replies to a user's question like an 8 Ball.")]
        [Example("8ball Will this losing streak end?!")]
        public async Task EightBall([Remainder][Summary("The question you wish for the Bot to respond to.")] 
            string question)
        {
            var responses = (await _eightBallResponseRepository.GetAll()).ToList();
            var randomIndex = _random.Next(responses.Count);

            await ReplyAsync(responses[randomIndex].Response);
        }

        #endregion
    }
}