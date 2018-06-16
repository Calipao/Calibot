using Discord;
using Discord.Commands;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using CalibotCS.Services.Reddit;

namespace CalibotCS.Modules
{
    [Name("Fun")]
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService m_commandService;
        public FunModule(CommandService _commandService)
        {
            m_commandService = _commandService;
        }

        [Command("8ball"), Alias("ball", "8b", "ask")]
        [Summary("Answer a question")]
        public async Task Eightball([Required, Remainder] string question = null)
        {
            string[] ans = new string[] {
                        "It is certain",
                        "It is decidedly so",
                        "Without a doubt",
                        "Yes, definitely",
                        "You may rely on it",
                        "As I see it, yes",
                        "Most likely",
                        "Outlook good",
                        "Yes",
                        "Signs point to yes",
                        "Reply hazy try again",
                        "Ask again later",
                        "Better not tell you now",
                        "Cannot predict now",
                        "Concentrate and ask again",
                        "Don't count on it",
                        "My reply is no",
                        "My sources say no",
                        "Outlook not so good",
                        "Very doubtful",
                        "If the stars align"
                    };
            EmbedBuilder eb = new EmbedBuilder
            {
                Description = $":8ball: **Question: {question}**\n **Ans: **{ans[new Random().Next(ans.Length)]}"
            };
            await ReplyAsync("", false, eb);
        }

        [Command("say"), Alias("echo")]
        [Summary("Echos a message")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("meme"), Alias("dank")]
        [Summary("Gives a dank meme")]
        public async Task SendMeme()
        {
            //Show the "typing...." status
            using (var typing = Context.Channel.EnterTypingState())
            {
                //Create a new RedditService
                var redditService = new RedditService();
                //Fetch the subreddit
                var subReddit = await redditService.GetSubredditAsync("dankmemes");
                //Fetch the posts
                var subRedditPosts = subReddit.Posts;

                //Randomly select a post
                Random rand = new Random();
                int r = rand.Next(subRedditPosts.Count);
               
                Console.WriteLine(string.Format("Subreddit Posts: {0}\n Random: {1}\n", subReddit.Posts.Count, r));
               
                var post = await redditService.GetPostAsync(subRedditPosts[r].Id);

                //Builds the message to return to user
                EmbedBuilder eb = new EmbedBuilder()
                {
                    Color = new Color(255, 200, 0),
                    Author = new EmbedAuthorBuilder()
                            .WithName($"{post.Title}")
                            .WithUrl($"https://www.reddit.com{post.Permalink}"),
                    ImageUrl = post.Url,
                    Footer = new EmbedFooterBuilder()
                            .WithText(string.Format("👍 {0} | 💬 {1}", post.Score, post.NumberOfComments))
                };

                await ReplyAsync("", false, eb);
            }
        }

        [Command("joke"), Alias("funny","jk")]
        [Summary("Gives u a bad joke")]
        public async Task SendJoke()
        {
            using (var typing = Context.Channel.EnterTypingState())
            {
                using (var httpClient = new HttpClient())
                {
                    int isImg = 1;
                    string link = $"http://www.amazingjokes.com/{(isImg == 1 ? "image" : "jokes")}/random";
                    var res = await httpClient.GetAsync(link);
                    if (!res.IsSuccessStatusCode)
                    {
                        await ReplyAsync($"An error occurred: {res.ReasonPhrase}");
                        typing.Dispose();
                        return;
                    }
                    string html = await res.Content.ReadAsStringAsync();
                    EmbedBuilder eb = new EmbedBuilder();
                    if (isImg == 1)
                    {
                        string[] ps = html.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None)[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                        eb.Author = new EmbedAuthorBuilder().WithName($"{ps[0]}");
                        eb.ImageUrl = ps[1].Split(new string[] { "og:image\" content=\"" }, StringSplitOptions.None)[1].Split('"')[0];
                    }
                    await ReplyAsync("", false, eb);
                }
            }
        }

        [Command("swag"), Alias("thug")]
        [Summary("Swags the chat")]
        public async Task Swag()
        {
            var msg = await ReplyAsync("( ͡° ͜ʖ ͡°)>⌐■-■");
            await Task.Delay(1500);
            await msg.ModifyAsync(x => { x.Content = "( ͡⌐■ ͜ʖ ͡-■)"; });
        }

        [Command("lenny"), Alias("lf")]
        [Summary("Posts a lenny face")]
        public async Task SendLenny()
        {
            await ReplyAsync("( ͡° ͜ʖ ͡°)");
        }
        
        [Command("hi"), Alias("hello", "hai", "hey")]
        [Summary("Says hi.")]
        public async Task SendHi()
        {
            await ReplyAsync("Hi, " + Context.Message.Author.Mention);
        }

        [Command("invite"), Alias("inv")]
        [Summary("Gives an invite link to invite Calibot to your own Guild!")]
        public async Task Invite()
        {
            var eb = new EmbedBuilder()
            {
                Title = "Invite Calibot",
                Color = new Color(4, 97, 247),
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Requested by {Context.User.Username}#{Context.User.Discriminator}",
                    IconUrl = (Context.User.GetAvatarUrl())
                },
                Description =
                    "[Click to Invite](https://discordapp.com/api/oauth2/authorize?client_id=428826752970784768&permissions=8&scope=bot)"
            };

            await ReplyAsync("", false, eb);
        }
    }
}
