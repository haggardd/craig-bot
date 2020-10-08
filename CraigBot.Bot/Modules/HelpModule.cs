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
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
    
        public HelpModule(CommandService commands, IConfigurationRoot config)
        {
            _commands = commands;
            _config = config;
        }

        #region Commands
        
        // TODO: Need to have a think about how commands are currently grouped, when to use optional parameters and how to relay this information to the user
        // TODO: Should include the summary and arguments to distinguish varieties of the same command
        [Command]
        public async Task Help()
        {
            var prefix = _config["prefix"];
            // TODO: Change these embed builders to use the methods opposed to properties
            var footer = new EmbedFooterBuilder()
            {
                Text = "Craig Bot, Brought to you by Discord.Net <3 - www.georgeblackwell.dev",
                IconUrl = "https://cdn.jsdelivr.net/gh/discord-net/Discord.Net/docs/marketing/logo/PackageLogo.png"
            };
            var embed = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = "All Commands:",
                Description = "Below is a list of all current commands. Some may be restricted based on your role.",
                Footer = footer
            };

            foreach (var module in _commands.Modules)
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
                    embed.AddField(x =>
                    {
                        x.Name = module.Summary;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, embed.Build());
        }
        
        // TODO: Will need to add a second command for getting more info on specific commands, i.e. !help flip etc...
        
        #endregion
    }
}