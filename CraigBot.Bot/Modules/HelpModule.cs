using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Modules
{
    [Summary("Help Commands")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly IConfigurationRoot _config;
    
        public HelpModule(CommandService commandService, IConfigurationRoot config)
        {
            _commandService = commandService;
            _config = config;
        }

        #region Commands
        
        // TODO: Need to have a think about how commands are currently grouped, when to use optional parameters or when to create an overload
        [Command("help")]
        [Summary("Displays a list of all commands.")]
        public async Task Help()
        {
            var prefix = _config["prefix"];
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle("All Commands: ")
                .WithDescription("Below is a list of all current commands. Some may be restricted based on your role.")
                .WithFooter(f =>
                {
                    f.Text = "Craig Bot, Brought to you by Discord.Net <3 / www.georgeblackwell.dev";
                    f.IconUrl = "https://cdn.jsdelivr.net/gh/discord-net/Discord.Net/docs/marketing/logo/PackageLogo.png";
                });

            foreach (var module in _commandService.Modules)
            {
                string description = null;

                foreach (var command in module.Commands)
                {
                    var result = await command.CheckPreconditionsAsync(Context);

                    if (result.IsSuccess)
                    {
                        description += $"`{prefix}{command.Aliases.First()}{string.Join("", command.Parameters.Select(p => $" [{p.Name}]"))}`\n";
                    }
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    embed.AddField(module.Summary, description);
                }
            }

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Relay info about parameters, if they're optional, arrays, require quotes etc...
        // TODO: Add summaries for parameters, check docs for this, its possible!
        // TODO: Finish this!
        [Command("help")]
        [Summary("Gives more information on specific commands.")]
        public async Task Help(string command)
        {
            var result = _commandService.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync("That command doesn't seem to exist! Try `!help` for a full list of commands.");
                return;
            }
            
            var prefix = _config["prefix"];
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle($"*{prefix}{command}*");

            foreach (var commandMatch in result.Commands)
            {
                embed.AddField(
                    $"`{prefix}{commandMatch.Command.Aliases.First()} {string.Join(" ", commandMatch.Command.Parameters.Select(p => $"[{p.Name}]"))}`", 
                    commandMatch.Command.Summary);
            }

            await ReplyAsync("", false, embed.Build());
        }
        
        #endregion
    }
}