using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Attributes;
using CraigBot.Bot.Configuration;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;

namespace CraigBot.Bot.Modules
{
    // TODO: Need to relay permissions for commands to the user
    [Summary("Help Commands")]
    public class HelpModule : CraigBotBaseModule
    {
        private readonly CommandService _commandService;
        private readonly BotOptions _options;
        
        public HelpModule(CommandService commandService, IOptions<BotOptions> options)
        {
            _commandService = commandService;
            _options = options.Value;
        }

        #region Commands
        
        [Command("help")]
        [Summary("Displays a list of all commands.")]
        public async Task Help()
        {
            var embed = BaseHelpEmbed()
                .WithTitle("Commands List")
                .WithDescription($"Use `{_options.Prefix}help [command]` for info on specific commands.")
                .WithFooter(f =>
                {
                    f.Text = "Craig Bot, Brought to you by Discord.Net <3 / www.georgeblackwell.dev";
                    f.IconUrl = "https://cdn.jsdelivr.net/gh/discord-net/Discord.Net/docs/marketing/logo/PackageLogo.png";
                });

            foreach (var module in _commandService.Modules)
            {
                var description = module.Commands.Aggregate<CommandInfo, string>(null, (current, command) 
                    => current + $"`{GenerateCommandParameterString(command)}`\n");

                if (!string.IsNullOrWhiteSpace(description))
                {
                    embed.AddField(module.Summary, description);
                }
            }

            await (_options.DmHelp ? Context.User.SendMessageAsync("", false, embed.Build())
                : ReplyAsync("", false, embed.Build()));
        }
        
        [Command("help")]
        [Summary("Gives information on specific commands.")]
        [Example("help poll")]
        public async Task Help([Remainder][Summary("The command name you wish to get more information about.")] 
            string command)
        {
            var result = _commandService.Search(Context, command);

            if (!result.IsSuccess)
            {
                await InlineReply(Context.Message, $"`{_options.Prefix}{command}` doesn't seem to exist, try `{_options.Prefix}help` for a full list of commands");
                return;
            }
            
            var embed = BaseHelpEmbed()
                .WithTitle($"*{_options.Prefix}{command}*");

            for (var i = 0; i < result.Commands.Count; i++)
            {
                var commandMatch = result.Commands[i];
                var commandInfo = commandMatch.Command;

                var commandHeader = i != 0
                    ? $"─────────────────────────────\n`{GenerateCommandParameterString(commandInfo)}`"
                    : $"`{GenerateCommandParameterString(commandInfo)}`";

                embed.AddField(
                    result.Commands.Count > 1
                        ? $"{commandHeader} - *Variant*"
                        : commandHeader, commandInfo.Summary);

                if (commandInfo.Parameters.Any())
                {
                    if (string.IsNullOrWhiteSpace(embed.Description))
                    {
                        embed.WithDescription("Key: `required` | *`optional`* | __`multiple`__");
                    }
                    
                    var parameterInfo = "";

                    foreach (var parameter in commandInfo.Parameters)
                    {
                        var properties = "";

                        properties += parameter.IsMultiple ? "__" : "";
                        properties += parameter.IsOptional ? "*" : "";

                        parameterInfo += $"• {properties}`[{parameter.Name}]`{properties} {parameter.Summary}\n";
                    }

                    embed.AddField("Parameters", parameterInfo);
                }

                var exampleInfo = commandInfo.Attributes.OfType<ExampleAttribute>().ToList();

                if (!exampleInfo.Any())
                {
                    continue;
                }

                var examples = "";
                examples = exampleInfo.Aggregate(examples, (current, example)
                    => current + $"• `{_options.Prefix}{example.ExampleText}`\n");

                embed.AddField("Examples", $"{examples}");
            }

            await (_options.DmHelp ? Context.User.SendMessageAsync("", false, embed.Build())
                : ReplyAsync("", false, embed.Build()));
        }
        
        #endregion

        #region Helpers

        private string GenerateCommandParameterString(CommandInfo command)
        {
            var parameters = string.Join("", 
                command.Parameters.Select(p => 
                    $" [{p.Name}]"
                ));

            return $"{_options.Prefix}{command.Aliases.First()}{parameters}";
        }

        private EmbedBuilder BaseHelpEmbed()
            => new EmbedBuilder()
                .WithColor(Color.Blue);

        #endregion
    }
}