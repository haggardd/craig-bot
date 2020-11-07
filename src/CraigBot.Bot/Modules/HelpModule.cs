using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Modules
{
    [Summary("Help Commands")]
    [RequireContext(ContextType.Guild)]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly IConfigurationRoot _config;
        
        private readonly bool _dmHelp;
        private readonly string _prefix;
    
        public HelpModule(CommandService commandService, IConfigurationRoot config)
        {
            _commandService = commandService;
            _config = config;

            _prefix = _config["Settings:Prefix"];
            _dmHelp = bool.Parse(_config["Settings:DmHelp"]);
        }

        #region Commands
        
        [Command("help")]
        [Summary("Displays a list of all commands.")]
        public async Task Help()
        {
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle("Commands List")
                .WithDescription($"Use `{_prefix}help [command]` for info on specific commands.")
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

            await (_dmHelp ? Context.User.SendMessageAsync("", false, embed.Build())
                : ReplyAsync("", false, embed.Build()));
        }
        
        [Command("help")]
        [Summary("Gives information on specific commands.")]
        [Example("help poll")]
        public async Task Help([Summary("The command name you wish to get more information about.")] 
            string command)
        {
            var result = _commandService.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"`{_prefix}{command}` doesn't seem to exist! Try `{_prefix}help` for a full list of commands.");
                return;
            }
            
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle($"*{_prefix}{command}*");

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
                    => current + $"• `{_prefix}{example.ExampleText}`\n");

                embed.AddField("Examples", $"{examples}");
            }

            await (_dmHelp ? Context.User.SendMessageAsync("", false, embed.Build())
                : ReplyAsync("", false, embed.Build()));
        }
        
        #endregion

        #region Helpers

        // TODO: May want to separate functions like this into a helper class
        private string GenerateCommandParameterString(CommandInfo command)
        {
            var parameters = string.Join("", 
                command.Parameters.Select(p => 
                    $" [{p.Name}]"
                ));

            return $"{_prefix}{command.Aliases.First()}{parameters}";
        }

        #endregion
    }
}