using System;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Domain.Repositories;
using Discord.Commands;

namespace CraigBot.Bot.Modules
{
    [Summary("Fun Commands")]
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _random;
        private readonly IStaticDataRepository _staticDataRepository;

        public FunModule(Random random, IStaticDataRepository staticDataRepository)
        {
            _random = random;
            _staticDataRepository = staticDataRepository;
        }
        
        #region Commands

        [Command("fortune")]
        [Summary("Replies with a random fortune.")]
        public async Task Fortune()
        {
            var fortunes = (await _staticDataRepository.Get("fortunes")).ToList();
            var randomIndex = _random.Next(fortunes.Count());

            await ReplyAsync(fortunes[randomIndex]);
        }

        [Command("8ball")]
        [Summary("Replies to a user's question like an 8 Ball.")]
        public async Task EightBall([Remainder]string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                await ReplyAsync("I can't give a proper response unless you ask me a question!");
                return;
            }
            
            var responses = (await _staticDataRepository.Get("eightBall")).ToList();
            var randomIndex = _random.Next(responses.Count());

            await ReplyAsync(responses[randomIndex]);
        }

        #endregion
    }
}