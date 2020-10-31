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

            _prefix = _config["prefix"];
            _dmHelp = bool.Parse(_config["settings:dmHelp"]);
        }

        #region Commands
        
        // TODO: Display if parameters are optional here too, no need for the summary though
        [Command("help")]
        [Summary("Displays a list of all commands.")]
        [Example("!help")]
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
        
        // TODO: Think of a good way to separate commands with the same name within the help embed, (currently only affects `!help`) 
        [Command("help")]
        [Summary("Gives more information on specific commands.")]
        [Example("!help poll")]
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
                .WithTitle($"*{_prefix}{command}*")
                .WithDescription($"\n*`({OptionalParam})` = Optional `({RequiredParam})` = Required `({MultipleParam})` = Multiple*\n");

            foreach (var commandMatch in result.Commands)
            {
                var commandInfo = commandMatch.Command;
                
                embed.AddField($"`{GenerateCommandParameterString(commandInfo)}`", commandInfo.Summary);

                if (commandInfo.Parameters.Any())
                {
                    var parameterInfo = "";

                    foreach (var parameter in commandInfo.Parameters)
                    {
                        var properties = "";

                        properties += parameter.IsOptional ? $"({OptionalParam})" : $"({RequiredParam})";
                        properties += parameter.IsMultiple ? $"({MultipleParam})" : "";
                    
                        parameterInfo += $"• `[{parameter.Name}]` `{properties}` {parameter.Summary}\n";
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
                    => current + $"• `{example.ExampleText}`\n  \n");

                embed.AddField("Examples", $"{examples}");
            }
            
            await (_dmHelp ? Context.User.SendMessageAsync("", false, embed.Build())
                : ReplyAsync("", false, embed.Build()));
        }
        
        #endregion

        #region Helpers

        // TODO: May want to relocate this to a separate file
        private const char OptionalParam = 'O';
        private const char RequiredParam = 'R';
        private const char MultipleParam = 'M';

        // TODO: May want to separate functions like this into a service
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