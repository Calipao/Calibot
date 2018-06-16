using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CalibotCS.Services.Reddit
{
    public class RedditService
    {
        private HttpClient client;
        public RedditService()
        {
            client = new HttpClient();
        }

        /*Returns a list of subreddits*/
        public async Task<IList<string>> GetSubredditsAsync()
        {
            var subreddits = new List<string>();
            var response = await client.GetStringAsync("http://www.reddit.com/.json");
            var jObject = JObject.Parse(response);
            var jArray = jObject["data"]["children"] as JArray;

            foreach (var obj in jArray)
            {
                subreddits.Add(obj["data"]["subreddit"].ToString());
            }

            subreddits = subreddits.OrderBy(sr => sr).ToList();

            return subreddits;
        }
        
        /*Returns a list of post in specified subreddit*/
        public async Task<Subreddit> GetSubredditAsync(string name)
        {
            var subreddit = new Subreddit();
            subreddit.Name = name;
            subreddit.Posts = new List<Post>();
            var response = await client.GetStringAsync("http://www.reddit.com/r/" + name + "/.json");
            var jObject = JObject.Parse(response);
            var jArray = jObject["data"]["children"] as JArray;

            foreach (var obj in jArray)
            {
                var post = new Post();

                post.Id = obj["data"]["id"].ToString();
                post.Title = obj["data"]["title"].ToString();
                post.Author = obj["data"]["author"].ToString();
                post.Score = obj["data"]["score"].Value<int>();
                post.Thumbnail = obj["data"]["thumbnail"].ToString();
                post.Url = obj["data"]["url"].ToString();
                post.Permalink = obj["data"]["permalink"].ToString();
                post.NumberOfComments = obj["data"]["num_comments"].Value<int>();
                post.Spoiler = obj["data"]["spoiler"].Value<bool>();
                post.Nsfw = obj["data"]["over_18"].Value<bool>();
                post.Pinned = obj["data"]["distinguished"].ToString();
                subreddit.Posts.Add(post);
            }

            return subreddit;
        }

        /*Returns a list of subreddits found from search*/
        public async Task<IList<string>> SearchForSubredditsAsync(string query)
        {
            var subreddits = new List<string>();
            var response = await client.GetStringAsync("http://www.reddit.com/subreddits/search.json?q=" + query);
            var jObject = JObject.Parse(response);
            var jArray = jObject["data"]["children"] as JArray;

            foreach (var obj in jArray)
            {
                subreddits.Add(obj["data"]["display_name"].ToString());
            }

            return subreddits;
        }

        /*Returns a post based on post id*/
        public async Task<Post> GetPostAsync(string id)
        {
            var post = new Post();
            var response = await client.GetStringAsync("http://www.reddit.com/api/info.json?id=t3_" + id);
            var jObject = JObject.Parse(response);
            var obj = (jObject["data"]["children"] as JArray)[0];

            post.Id = obj["data"]["id"].ToString();
            post.Title = obj["data"]["title"].ToString();
            post.Author = obj["data"]["author"].ToString();
            post.Score = obj["data"]["score"].Value<int>();
            post.Thumbnail = obj["data"]["thumbnail"].ToString();
            post.Url = obj["data"]["url"].ToString();
            post.Permalink = obj["data"]["permalink"].ToString();
            post.NumberOfComments = obj["data"]["num_comments"].Value<int>();
            post.Spoiler = obj["data"]["spoiler"].Value<bool>();
            post.Nsfw = obj["data"]["over_18"].Value<bool>();
            post.Pinned = obj["data"]["distinguished"].ToString();

            return post;
        }
    }
}
