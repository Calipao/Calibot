using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace CalibotCS.Modules
{
    [Name("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService m_service;
        public HelpModule(CommandService _service)
        {
            m_service = _service;
        }
        
        [Command("help"), Alias("h")]
        [Summary("Sends help")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(255, 200, 0),
                Description = "**Hello! I'm Calibot!**\nPrefix: `plz`",
            };

            //Loop through all modules
            foreach (var module in m_service.Modules)
            {
                string description = null;
                //Skip the help module
                if (module.Name.Equals("Help")) continue;

                //Loop through commands in this module
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"`{cmd.Aliases.First()}`, ";
                }
                description = description.Remove(description.Length - 2, 2);

                //String isnt empty or null
                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            builder.AddField(x =>
            {
                x.Name = "Help";
                x.Value = "type `help <command>` for more info";
                x.IsInline = false;
            });

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help"), Alias("h")]
        [Summary("Sends more help")]
        public async Task HelpAsync(string _command)
        {
            var result = m_service.Search(Context, _command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{_command}**.");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = new Color(255, 200, 0),
                Description = $"Aliases of **{_command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
