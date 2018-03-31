using Discord;
using Discord.WebSocket;
using Discord.Commands;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalibotCS.Modules
{
    [Name("General")]
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService m_service;
        public GeneralModule(CommandService _service)
        {
            m_service = _service;
        }

        [Command("ping"), Alias("p", "ms")]
        [Summary("Check latency")]
        public async Task SendPing()
        {
            await ReplyAsync($"🏓 Pong! ``{(Context.Client as DiscordSocketClient).Latency}ms``");
        }

        [Command("about")]
        [Summary("Gives an about page")]
        public async Task AboutInfo()
        {
            var eb = new EmbedBuilder()
            {
                Title = "About",
                Color = new Color(4, 97, 247),
                ThumbnailUrl = (Context.Client.CurrentUser.GetAvatarUrl()),
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Requested by {Context.User.Username}#{Context.User.Discriminator}",
                    IconUrl = (Context.User.GetAvatarUrl())
                }
            };
            eb.AddField((efb) =>
            {
                efb.Name = "Creator";
                efb.IsInline = true;
                efb.Value =
                    "This bot has been created by Calipao#5292.\n";
            });
            eb.AddField((efb) =>
            {
                efb.Name = "Properties";
                efb.IsInline = true;
                efb.Value =
                    "I was written in C# using the Discord.NET 1.0.2 API.\n" +
                    "For help about this bot use the `help` command\n";
            });
            eb.AddField((efb) =>
            {
                efb.Name = "About me";
                efb.IsInline = true;
                efb.Value = "I created this bot for fun since I had nothing better to do.\n" +
                            "My name is Lin Xin and I'm a student.\n" +
                            "My birthday is on the 27th of May and I'm currently 18 years old.\n";
            });
            await ReplyAsync("", false, eb);
        }

        [Command("clean"), Alias("clear")]
        [Summary("Delete all the messages from this bot within the last X messages")]
        public async Task Clean([Summary("Number of message to delete")]int numMessages = 30)
        {
            if (numMessages > 50)
                numMessages = 50;
            else if (numMessages < 2)
                numMessages = 2;
            var msgs = await Context.Channel.GetMessagesAsync(numMessages).Flatten();
            msgs = msgs.Where(x => x.Author.Id == Context.Client.CurrentUser.Id);
            foreach (IMessage msg in msgs)
                await msg.DeleteAsync();
        }
    }
}
