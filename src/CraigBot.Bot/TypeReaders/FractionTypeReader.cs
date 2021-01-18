using System;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord.Commands;

namespace CraigBot.Bot.TypeReaders
{
    public class FractionTypeReader : TypeReader
    {
        public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, 
            IServiceProvider services)
        {
            if (!Fraction.TryParse(input, out var result))
            {
                return await Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed,
                    "Input could not be parsed as a fraction."));
            }

            return await Task.FromResult(TypeReaderResult.FromSuccess(result));
        }
    }
}