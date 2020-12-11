using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace CraigBot.Bot.TypeReaders
{
    public class CurrencyTypeReader : TypeReader
    {
        public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, 
            IServiceProvider services)
        {
            if (!decimal.TryParse(input, out var result))
            {
                return await Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed,
                    "Input could not be parsed as currency."));
            }

            return await Task.FromResult(TypeReaderResult.FromSuccess(Math.Round(result, 2)));
        }
    }
}