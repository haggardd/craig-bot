using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace CraigBot.Bot.Modules
{    
    [Group("help")]
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
        
        // TODO: Need to have a think about how commands are currently grouped, when to use optional parameters and how to relay this information to the user
        [Command]
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
                        description += $"{prefix}{command.Aliases.First()} {string.Join(" ", command.Parameters.Select(p => $"`[{p.Name}]`"))} \n";
                    }
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    embed.AddField(f =>
                    {
                        f.Name = module.Summary;
                        f.Value = description;
                        f.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Will need to add a second command for getting more info on specific commands, i.e. !help flip etc...
        
        #endregion
    }
}